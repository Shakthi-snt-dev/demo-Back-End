using FlowTap.Api.Data;
using FlowTap.Api.Extensions;
using FlowTap.Api.Middleware;
using FlowTap.Api.Models;
using FlowTap.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Cryptography;
using System.Text.Json;
using Hangfire;
using Hangfire.PostgreSql;
using StackExchange.Redis;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Serilog Configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq") ?? "http://localhost:5341")
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger Configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FlowTap POS & Repair Management API",
        Version = "v1",
        Description = "Complete backend API for FlowTap POS & Repair Management System"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// ASP.NET Identity Configuration
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// JWT Configuration with RSA
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var rsaPrivateKey = RSA.Create();
var rsaPublicKey = RSA.Create();

// Load or generate RSA keys
var privateKeyPath = Path.Combine(Directory.GetCurrentDirectory(), "keys", "private_key.pem");
var publicKeyPath = Path.Combine(Directory.GetCurrentDirectory(), "keys", "public_key.pem");

if (File.Exists(privateKeyPath) && File.Exists(publicKeyPath))
{
    rsaPrivateKey.ImportFromPem(File.ReadAllText(privateKeyPath));
    rsaPublicKey.ImportFromPem(File.ReadAllText(publicKeyPath));
}
else
{
    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "keys"));
    var privateKeyPem = rsaPrivateKey.ExportRSAPrivateKeyPem();
    var publicKeyPem = rsaPublicKey.ExportRSAPublicKeyPem();
    File.WriteAllText(privateKeyPath, privateKeyPem);
    File.WriteAllText(publicKeyPath, publicKeyPem);
}

var rsaSecurityKey = new RsaSecurityKey(rsaPrivateKey) { KeyId = "FlowTapKey" };

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = rsaSecurityKey,
        ClockSkew = TimeSpan.Zero
    };
});

// Redis Configuration (Optional)
var redisConnection = builder.Configuration.GetConnectionString("Redis");
if (!string.IsNullOrEmpty(redisConnection))
{
    try
    {
        builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(redisConnection));
    }
    catch
    {
        // Redis is optional, continue without it
    }
}

// Hangfire Configuration (Optional - only register if we can connect)
bool hangfireEnabled = false;
try
{
    // Test connection before registering Hangfire
    using var testConnection = new Npgsql.NpgsqlConnection(connectionString);
    testConnection.Open();
    testConnection.Close();
    hangfireEnabled = true;
    
    builder.Services.AddHangfire(config =>
        config.UsePostgreSqlStorage(connectionString));
    builder.Services.AddHangfireServer();
    Console.WriteLine("Hangfire configured successfully.");
}
catch (Exception ex)
{
    // Hangfire is optional, log and continue
    Console.WriteLine($"Warning: Hangfire could not be configured: {ex.Message}");
    Console.WriteLine("The application will continue without Hangfire background jobs.");
}

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDashboardService>(sp =>
{
    var context = sp.GetRequiredService<ApplicationDbContext>();
    var redis = sp.GetService<IConnectionMultiplexer>();
    return new DashboardService(context, redis);
});
builder.Services.AddScoped<IPosService, PosService>();
builder.Services.AddScoped<IRepairService, RepairService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IIntegrationService, IntegrationService>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISmsService, SmsService>();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlowTap API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

// Hangfire Dashboard (only if configured)
if (app.Environment.IsDevelopment() && hangfireEnabled)
{
    try
    {
        app.UseHangfireDashboard("/hangfire");
    }
    catch
    {
        // Hangfire not available, skip dashboard
    }
}

// Apply migrations on startup (with error handling)
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        // Test connection first
        if (dbContext.Database.CanConnect())
        {
            dbContext.Database.Migrate();
            
            // Seed initial data
            await SeedData.InitializeAsync(scope.ServiceProvider);
            Console.WriteLine("Database initialized successfully.");
        }
        else
        {
            Console.WriteLine("Warning: Cannot connect to database. Please check your connection string.");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Warning: Database initialization failed: {ex.Message}");
    Console.WriteLine("The application will continue, but database features may not work.");
    Console.WriteLine("Please check your PostgreSQL connection string in appsettings.json");
}

app.Run();

