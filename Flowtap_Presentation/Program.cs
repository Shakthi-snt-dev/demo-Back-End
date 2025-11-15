using Serilog;
using System.Reflection;
using Flowtap_Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, config) =>
{
    var appName = Assembly.GetExecutingAssembly().GetName().Name;
    var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

    // Construct the log directory path dynamically
    var logDirectory = $"Log/{appName}-{version}";

    // Create log file path inside the container
    var logFilePath = $"{logDirectory}/log.txt"; // inside container (mountable)

    Directory.CreateDirectory(logDirectory); // safe even if volume is mounted

    config
        .ReadFrom.Configuration(context.Configuration) // read from appsettings.json if available
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", appName)
        .WriteTo.Console( // Docker reads logs from console
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
        .WriteTo.File( //  optional file log (if volume mounted)
            Path.Combine(logFilePath, "log-.txt"),
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 7,
            shared: true);
});

// Add API Configuration (includes Application, Infrastructure, Presentation, Swagger, etc.)
builder.Services.AddApiConfiguration(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CustomCorsPolicy");

// Exception handling middleware
app.UseMiddleware<Flowtap_Middleware.ExceptionMiddleware.RecoveryHandler>();

// Authentication and Authorization (only if configured)
// These will work even if JWT is not configured (they'll just be no-ops)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
