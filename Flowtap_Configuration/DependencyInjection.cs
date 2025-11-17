using Flowtap_Application;
using Flowtap_Application.Mapping;
using Flowtap_Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;


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
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                }
            );
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


            });
            #endregion
            #region ConfigAuthorization
            // Always add authorization services (required for UseAuthorization middleware)
            services.AddAuthorization();
            #endregion

            #region ConfigJwtAsymmetric
            var publicKeyPath = configuration.GetValue<string>("Tokens:PublicKey");
            if (string.IsNullOrWhiteSpace(publicKeyPath))
            {
                // If no public key path is configured, add authentication without JWT
                // This allows the app to run without JWT keys for development
                services.AddAuthentication();
                return services;
            }

            RSA publicRsa = RSA.Create();
            var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                "Keys",
                publicKeyPath);
            
            if (!File.Exists(filePath))
            {
                // If key file doesn't exist, skip JWT configuration
                return services;
            }

            var data = File.ReadAllText(filePath);
            publicRsa.ImportFromPem(data);
            RsaSecurityKey signingKey = new RsaSecurityKey(publicRsa);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.RequireHttpsMetadata = false;
                option.SaveToken = true;
                /*                option.Authority = "https://securetoken.google.com/skillset-4c0a7";
                */
                option.TokenValidationParameters = new TokenValidationParameters()
                {

                    IssuerSigningKey = signingKey,
                    ValidateAudience = true,
                    ValidAudience = configuration.GetValue<string>("Tokens:Audience"),
                    ValidateIssuer = true,
                    ValidIssuer = configuration.GetValue<string>("Tokens:Issuer"),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });


            #endregion

            return services;
        }
    }
}