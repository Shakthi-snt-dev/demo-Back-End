using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Identity.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;

namespace Flowtap_Application.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtService> _logger;

    public JwtService(IConfiguration configuration, ILogger<JwtService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public string GenerateToken(UserAccount userAccount)
    {
        return GenerateToken(userAccount, userAccount.AppUserId, null);
    }

    public string GenerateToken(UserAccount userAccount, Guid? appUserId)
    {
        return GenerateToken(userAccount, appUserId, null);
    }

    public string GenerateToken(UserAccount userAccount, Guid? appUserId, Guid? storeId)
    {
        try
        {
            var issuer = _configuration["Tokens:Issuer"] ?? "http://localhost:5000";
            var audience = _configuration["Tokens:Audience"] ?? "http://localhost:4000";
            var lifetime = int.Parse(_configuration["Tokens:LifeTime"] ?? "4320"); // minutes

            // Load private key for signing
            var privateKeyPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Keys",
                _configuration["Tokens:PrivateKey"] ?? "privatekey.pem");

            if (!File.Exists(privateKeyPath))
            {
                _logger.LogWarning("Private key file not found at {Path}, using symmetric key fallback", privateKeyPath);
                return GenerateSymmetricToken(userAccount, appUserId, storeId, issuer, audience, lifetime);
            }

            var privateKeyData = File.ReadAllText(privateKeyPath);
            
            // Create RSA and extract parameters to avoid disposal issues
            RSAParameters rsaParams;
            using (var rsa = RSA.Create())
            {
                rsa.ImportFromPem(privateKeyData);
                rsaParams = rsa.ExportParameters(true); // Export private key parameters
            }
            
            // Create a new RSA instance from the exported parameters
            var rsaForSigning = RSA.Create();
            rsaForSigning.ImportParameters(rsaParams);
            var signingKey = new RsaSecurityKey(rsaForSigning);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userAccount.Id.ToString()),
                new Claim(ClaimTypes.Email, userAccount.Email),
                new Claim("user_type", userAccount.UserType.ToString()),
                new Claim("is_email_verified", userAccount.IsEmailVerified.ToString().ToLower()),
            };

            if (!string.IsNullOrWhiteSpace(userAccount.Username))
            {
                claims.Add(new Claim(ClaimTypes.Name, userAccount.Username));
            }

            if (appUserId.HasValue)
            {
                claims.Add(new Claim("app_user_id", appUserId.Value.ToString()));
            }

            // Add store_id claim if provided
            if (storeId.HasValue && storeId.Value != Guid.Empty)
            {
                claims.Add(new Claim("store_id", storeId.Value.ToString()));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(lifetime),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating JWT token");
            // Fallback to symmetric key
            return GenerateSymmetricToken(userAccount, appUserId, storeId,
                _configuration["Tokens:Issuer"] ?? "http://localhost:5000",
                _configuration["Tokens:Audience"] ?? "http://localhost:4000",
                int.Parse(_configuration["Tokens:LifeTime"] ?? "4320"));
        }
    }

    private string GenerateSymmetricToken(UserAccount userAccount, Guid? appUserId, Guid? storeId, string issuer, string audience, int lifetime)
    {
        var secretKey = _configuration["Tokens:SecretKey"] ?? "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userAccount.Id.ToString()),
            new Claim(ClaimTypes.Email, userAccount.Email),
            new Claim("user_type", userAccount.UserType.ToString()),
            new Claim("is_email_verified", userAccount.IsEmailVerified.ToString().ToLower()),
        };

        if (!string.IsNullOrWhiteSpace(userAccount.Username))
        {
            claims.Add(new Claim(ClaimTypes.Name, userAccount.Username));
        }

        if (appUserId.HasValue)
        {
            claims.Add(new Claim("app_user_id", appUserId.Value.ToString()));
        }

        // Add store_id claim if provided
        if (storeId.HasValue && storeId.Value != Guid.Empty)
        {
            claims.Add(new Claim("store_id", storeId.Value.ToString()));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(lifetime),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

