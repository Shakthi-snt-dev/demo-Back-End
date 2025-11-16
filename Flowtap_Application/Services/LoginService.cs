using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Identity.Interfaces;
using Flowtap_Domain.BoundedContexts.Owner.Interfaces;
using Flowtap_Domain.DtoModel;
using Flowtap_Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Flowtap_Application.Services;

public class LoginService : ILoginService
{
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IJwtService _jwtService;
    private readonly ILogger<LoginService> _logger;

    public LoginService(
        IUserAccountRepository userAccountRepository,
        IAppUserRepository appUserRepository,
        IJwtService jwtService,
        ILogger<LoginService> logger)
    {
        _userAccountRepository = userAccountRepository;
        _appUserRepository = appUserRepository;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        // Find user by email
        var userAccount = await _userAccountRepository.GetByEmailAsync(request.Email);
        
        if (userAccount == null)
        {
            _logger.LogWarning("Login attempt with non-existent email: {Email}", request.Email);
            return new LoginResponseDto
            {
                Success = false,
                Message = "Invalid email or password."
            };
        }

        // Check if account is active
        if (!userAccount.IsActive)
        {
            _logger.LogWarning("Login attempt with inactive account: {Email}", request.Email);
            throw new AccountDeactivatedException(request.Email);
        }

        // Check if email is verified
        if (!userAccount.IsEmailVerified)
        {
            _logger.LogWarning("Login attempt with unverified email: {Email}", request.Email);
            throw new EmailNotVerifiedException(request.Email);
        }

        // Validate password
        var isPasswordValid = await ValidatePasswordAsync(
            request.Password, 
            userAccount.PasswordHash, 
            userAccount.PasswordSalt);

        if (!isPasswordValid)
        {
            _logger.LogWarning("Invalid password attempt for email: {Email}", request.Email);
            return new LoginResponseDto
            {
                Success = false,
                Message = "Invalid email or password."
            };
        }

        // Update last login timestamp
        userAccount.UpdateLastLogin();
        await _userAccountRepository.UpdateAsync(userAccount);

        // Get AppUser if exists
        var appUser = userAccount.AppUserId.HasValue
            ? await _appUserRepository.GetByIdAsync(userAccount.AppUserId.Value)
            : null;

        // Get primary store ID (first store from StoreIds collection)
        Guid? primaryStoreId = null;
        if (appUser != null && appUser.StoreIds != null && appUser.StoreIds.Any())
        {
            primaryStoreId = appUser.StoreIds.FirstOrDefault();
        }

        // Generate JWT token with store ID
        var token = _jwtService.GenerateToken(userAccount, userAccount.AppUserId, primaryStoreId);

        _logger.LogInformation("Successful login for email: {Email}, UserId: {UserId}", request.Email, userAccount.Id);

        return new LoginResponseDto
        {
            Success = true,
            Message = "Login successful.",
            Token = token,
            User = new UserInfoDto
            {
                UserId = userAccount.Id,
                AppUserId = userAccount.AppUserId,
                Email = userAccount.Email,
                Username = userAccount.Username,
                UserType = userAccount.UserType.ToString(),
                IsEmailVerified = userAccount.IsEmailVerified,
                LastLoginAt = userAccount.LastLoginAt
            }
        };
    }

    public async Task<bool> ValidatePasswordAsync(string password, string passwordHash, string? passwordSalt)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordHash))
        {
            return false;
        }

        // Hash the provided password with the stored salt
        using var sha256 = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(password + (passwordSalt ?? ""));
        var hashBytes = sha256.ComputeHash(passwordBytes);
        var computedHash = Convert.ToBase64String(hashBytes);

        // Compare hashes (constant-time comparison to prevent timing attacks)
        return ConstantTimeEquals(computedHash, passwordHash);
    }

    private bool ConstantTimeEquals(string a, string b)
    {
        if (a.Length != b.Length)
            return false;

        var result = 0;
        for (var i = 0; i < a.Length; i++)
        {
            result |= a[i] ^ b[i];
        }

        return result == 0;
    }
}

