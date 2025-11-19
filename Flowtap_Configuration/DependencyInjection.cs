using Flowtap_Application;
using Flowtap_Application.Mapping;
using Flowtap_Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Any;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;


namespace Flowtap_Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            #region Application
            services.AddApplication();
            #endregion

            #region Infrastructure
            services.AddInfrastructure(configuration);
            #endregion

            #region Controllers
            services.AddControllers();
            #endregion

            #region HttpContextAccessor
            services.AddHttpContextAccessor();
            #endregion

            services.AddEndpointsApiExplorer();
            #region ConfigCors
            services.AddCors(option => option.AddPolicy("CustomCorsPolicy", x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
            #endregion
            #region ConfigMapper
            services.AddAutoMapper(typeof(MappingProfile));
            #endregion
            #region ADDSWAGGERAUTHORITY
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Flowtap API",
                    Version = "v1",
                    Description = "Flowtap API Documentation"
                });

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });

                // Custom schema options to handle complex types and avoid conflicts
                swagger.CustomSchemaIds(type => 
                {
                    if (type == null) return "UnknownType";
                    var name = type.FullName ?? type.Name;
                    if (string.IsNullOrEmpty(name)) return "UnknownType";
                    
                    // Clean up the name for Swagger
                    name = name.Replace("+", ".")
                               .Replace("`", "_")
                               .Replace("[", "_")
                               .Replace("]", "_");
                    
                    return name;
                });
                
                // Map IFormFile to file type for Swagger
                swagger.MapType<IFormFile>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                });
                
                // Handle form file uploads properly - custom filters
                // Note: Operation filter runs after parameter generation, so we need to handle this differently
                swagger.OperationFilter<FormFileOperationFilter>();
                swagger.ParameterFilter<FormFileParameterFilter>();
                
                // Configure to handle form files properly
                swagger.CustomOperationIds(apiDesc => 
                {
                    return apiDesc.ActionDescriptor.RouteValues["action"];
                });
                
                // Ignore obsolete properties to reduce schema complexity
                swagger.IgnoreObsoleteProperties();
            });
            #endregion
            #region ConfigAuthorization
            // Always add authorization services (required for UseAuthorization middleware)
            services.AddAuthorization();
            #endregion

            #region ConfigJwtAsymmetric
            var publicKeyPath = configuration.GetValue<string>("Tokens:PublicKey");
            var secretKey = configuration.GetValue<string>("Tokens:SecretKey") ?? "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!";
            
            // Always configure authentication with JWT Bearer as default
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.RequireHttpsMetadata = false;
                option.SaveToken = true;
                
                var tokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = configuration.GetValue<string>("Tokens:Audience") ?? "http://localhost:4000",
                    ValidateIssuer = true,
                    ValidIssuer = configuration.GetValue<string>("Tokens:Issuer") ?? "http://localhost:5000",
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    // Map JWT claim names to ASP.NET Core claim types
                    NameClaimType = "nameid", // Map nameid to Name claim
                    RoleClaimType = ClaimTypes.Role
                };

                // Try to use RSA (asymmetric) if public key exists
                if (!string.IsNullOrWhiteSpace(publicKeyPath))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Keys", publicKeyPath);
                    
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            RSA publicRsa = RSA.Create();
                            var data = File.ReadAllText(filePath);
                            publicRsa.ImportFromPem(data);
                            RsaSecurityKey signingKey = new RsaSecurityKey(publicRsa);
                            tokenValidationParameters.IssuerSigningKey = signingKey;
                            Console.WriteLine("RSA public key loaded successfully");
                        }
                        catch (Exception ex)
                        {
                            // If RSA key loading fails, fall back to symmetric key
                            Console.WriteLine($"Warning: Failed to load RSA public key, falling back to symmetric key: {ex.Message}");
                            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                            tokenValidationParameters.IssuerSigningKey = symmetricKey;
                        }
                    }
                    else
                    {
                        // Public key file doesn't exist, use symmetric key
                        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                        tokenValidationParameters.IssuerSigningKey = symmetricKey;
                    }
                }
                else
                {
                    // No public key configured, use symmetric key
                    var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                    tokenValidationParameters.IssuerSigningKey = symmetricKey;
                }

                option.TokenValidationParameters = tokenValidationParameters;
            });

            #endregion

            return services;
        }
    }

    /// <summary>
    /// Operation filter to handle IFormFile parameters with [FromForm] attribute
    /// </summary>
    public class FormFileOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Find all IFormFile parameters
            var formFileParams = context.ApiDescription.ParameterDescriptions
                .Where(p => p.Type == typeof(IFormFile) || p.Type == typeof(IFormFileCollection))
                .ToList();

            if (!formFileParams.Any())
                return;

            // Remove IFormFile parameters from the parameters list
            if (operation.Parameters != null)
            {
                var toRemove = operation.Parameters
                    .Where(p => formFileParams.Any(fp => fp.Name == p.Name))
                    .ToList();
                
                foreach (var param in toRemove)
                {
                    operation.Parameters.Remove(param);
                }
            }

            // Create or update request body
            if (operation.RequestBody == null)
            {
                operation.RequestBody = new OpenApiRequestBody();
            }

            if (operation.RequestBody.Content == null)
            {
                operation.RequestBody.Content = new Dictionary<string, OpenApiMediaType>();
            }

            // Create multipart/form-data schema
            if (!operation.RequestBody.Content.ContainsKey("multipart/form-data"))
            {
                operation.RequestBody.Content["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>()
                    }
                };
            }

            var mediaType = operation.RequestBody.Content["multipart/form-data"];
            if (mediaType.Schema.Properties == null)
            {
                mediaType.Schema.Properties = new Dictionary<string, OpenApiSchema>();
            }

            // Add file parameters to the schema
            foreach (var paramDesc in formFileParams)
            {
                var fileSchema = paramDesc.Type == typeof(IFormFileCollection)
                    ? new OpenApiSchema
                    {
                        Type = "array",
                        Items = new OpenApiSchema
                        {
                            Type = "string",
                            Format = "binary"
                        }
                    }
                    : new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary"
                    };

                mediaType.Schema.Properties[paramDesc.Name] = fileSchema;
            }
        }
    }

    /// <summary>
    /// Parameter filter to handle IFormFile parameters - prevents Swashbuckle from trying to generate them as query parameters
    /// </summary>
    public class FormFileParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            // If this is an IFormFile parameter, we need to skip it here
            // The operation filter will handle it in the request body
            if (context.ApiParameterDescription?.Type == typeof(IFormFile) || 
                context.ApiParameterDescription?.Type == typeof(IFormFileCollection))
            {
                // This will be handled by the operation filter, so we can leave it
                // The operation filter will remove it from parameters
            }
        }
    }
}