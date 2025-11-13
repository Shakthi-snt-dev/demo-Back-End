using FlowTap.Api.Data;
using FlowTap.Api.DTOs;
using FlowTap.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FlowTap.Api.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly RSA _rsaPrivateKey;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager,
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _context = context;
        _configuration = configuration;

        // Load RSA private key
        var privateKeyPath = Path.Combine(Directory.GetCurrentDirectory(), "keys", "private_key.pem");
        _rsaPrivateKey = RSA.Create();
        if (File.Exists(privateKeyPath))
        {
            _rsaPrivateKey.ImportFromPem(File.ReadAllText(privateKeyPath));
        }
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            Username = request.Username,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        // Assign default role (Cashier)
        var defaultRole = await _roleManager.FindByNameAsync("Cashier");
        if (defaultRole != null)
        {
            user.RoleId = defaultRole.Id;
            await _userManager.UpdateAsync(user);
        }

        return await GenerateAuthResponseAsync(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        return await GenerateAuthResponseAsync(user);
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        // Simplified refresh token - in production, store refresh tokens in database
        var principal = GetPrincipalFromExpiredToken(request.Token);
        var userId = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid token");
        }

        return await GenerateAuthResponseAsync(user);
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            // Don't reveal if user exists
            return true;
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        // In production, send email with reset link
        return true;
    }

    public async Task<UserDto> GetCurrentUserAsync(Guid userId)
    {
        var user = await _userManager.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email ?? string.Empty,
            ProfilePicture = user.ProfilePicture,
            RoleName = user.Role?.Name
        };
    }

    public async Task<UserDto> UpdateProfileAsync(Guid userId, UpdateProfileRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (!string.IsNullOrEmpty(request.Username))
            user.Username = request.Username;
        if (!string.IsNullOrEmpty(request.ProfilePicture))
            user.ProfilePicture = request.ProfilePicture;
        if (!string.IsNullOrEmpty(request.AccessPin))
            user.AccessPin = request.AccessPin;

        user.UpdatedAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return await GetCurrentUserAsync(userId);
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        return result.Succeeded;
    }

    private async Task<AuthResponse> GenerateAuthResponseAsync(ApplicationUser user)
    {
        var userDto = await GetCurrentUserAsync(user.Id);
        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        return new AuthResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60")),
            User = userDto
        };
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (user.RoleId.HasValue)
        {
            claims.Add(new Claim(ClaimTypes.Role, user.Role?.Name ?? ""));
        }

        var key = new RsaSecurityKey(_rsaPrivateKey) { KeyId = "FlowTapKey" };
        var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60")),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new RsaSecurityKey(_rsaPrivateKey),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.RsaSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}

