using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Owner.Interfaces;
using Flowtap_Domain.BoundedContexts.Store.Interfaces;
using Flowtap_Domain.BoundedContexts.Identity.Interfaces;
using Flowtap_Domain.BoundedContexts.HR.Interfaces;
using Flowtap_Domain.BoundedContexts.HR.Entities;
using Flowtap_Domain.DtoModel;
using Flowtap_Domain.Exceptions;
using Flowtap_Domain.SharedKernel.Enums;
using Flowtap_Domain.SharedKernel.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace Flowtap_Application.Services;

/// <summary>
/// Profile Service - Manage user profile settings
/// </summary>
public class ProfileService : IProfileService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<ProfileService> _logger;

    public ProfileService(
        IAppUserRepository appUserRepository,
        IStoreRepository storeRepository,
        IUserAccountRepository userAccountRepository,
        IEmployeeRepository employeeRepository,
        ILogger<ProfileService> logger)
    {
        _appUserRepository = appUserRepository;
        _storeRepository = storeRepository;
        _userAccountRepository = userAccountRepository;
        _employeeRepository = employeeRepository;
        _logger = logger;
    }

    public async Task<AppUserProfileResponseDto> GetAppUserProfileAsync(Guid appUserId)
    {
        var appUser = await _appUserRepository.GetByIdAsync(appUserId);
        if (appUser == null)
            throw new EntityNotFoundException("AppUser", appUserId);

        var userAccount = await _userAccountRepository.GetByEmailAsync(appUser.Email);
        if (userAccount == null)
            throw new EntityNotFoundException("UserAccount", "Email", appUser.Email);

        // Get stores for this AppUser
        var stores = await _storeRepository.GetByAppUserIdAsync(appUserId);
        // Use DefaultStoreId from AppUser if set, otherwise fall back to first store
        var defaultStoreId = appUser.DefaultStoreId ?? stores.FirstOrDefault()?.Id;

        // Extract address components
        var address = appUser.Address;

        return new AppUserProfileResponseDto
        {
            Username = userAccount.Username,
            Email = appUser.Email,
            Language = appUser.Language,
            Phone = appUser.PhoneNumber,
            Mobile = appUser.PhoneNumber, // Using PhoneNumber as mobile
            StreetNumber = address?.StreetNumber,
            StreetName = address?.StreetName,
            City = address?.City,
            State = address?.State,
            Country = appUser.Country,
            PostalCode = address?.PostalCode,
            DefaultStoreId = defaultStoreId,
            EnableTwoFactor = userAccount.TwoFactorEnabled
        };
    }

    public async Task<AppUserProfileResponseDto> CreateOrUpdateAppUserProfileAsync(Guid appUserId, AppUserProfileRequestDto request)
    {
        var appUser = await _appUserRepository.GetByIdAsync(appUserId);
        if (appUser == null)
            throw new EntityNotFoundException("AppUser", appUserId);

        var userAccount = await _userAccountRepository.GetByEmailAsync(appUser.Email);
        if (userAccount == null)
            throw new EntityNotFoundException("UserAccount", "Email", appUser.Email);

        // Update Username
        if (request.Username != null)
        {
            userAccount.Username = request.Username;
        }

        // Note: Email should NOT be updated via PUT profile endpoint for security reasons
        // Email changes should be handled through a separate email verification flow

        // Update Phone
        if (request.Phone != null)
        {
            appUser.PhoneNumber = request.Phone;
        }

        // Update Mobile (using PhoneNumber field)
        if (request.Mobile != null)
        {
            appUser.PhoneNumber = request.Mobile;
        }

        // Update Language
        if (request.Language != null)
        {
            appUser.Language = request.Language;
        }

        // Update Country
        if (request.Country != null)
        {
            appUser.Country = request.Country;
        }

        // Update Address
        if (request.StreetNumber != null || request.StreetName != null || 
            request.City != null || request.State != null || request.PostalCode != null)
        {
            var existingAddress = appUser.Address;
            var newAddress = new Address(
                request.StreetNumber ?? existingAddress?.StreetNumber ?? string.Empty,
                request.StreetName ?? existingAddress?.StreetName ?? string.Empty,
                request.City ?? existingAddress?.City ?? string.Empty,
                request.State ?? existingAddress?.State ?? string.Empty,
                request.PostalCode ?? existingAddress?.PostalCode ?? string.Empty);

            appUser.UpdateAddress(newAddress);
        }

        // Update Two-Factor Authentication
        if (request.EnableTwoFactor.HasValue)
        {
            if (request.EnableTwoFactor.Value && !userAccount.TwoFactorEnabled)
            {
                userAccount.EnableTwoFactor();
            }
            else if (!request.EnableTwoFactor.Value && userAccount.TwoFactorEnabled)
            {
                userAccount.DisableTwoFactor();
            }
        }

        // Update default store if provided
        if (request.DefaultStoreId.HasValue)
        {
            appUser.SetDefaultStore(request.DefaultStoreId.Value);
        }

        // Save changes
        await _appUserRepository.UpdateAsync(appUser);
        await _userAccountRepository.UpdateAsync(userAccount);

        // Get updated stores
        var stores = await _storeRepository.GetByAppUserIdAsync(appUserId);
        // Use DefaultStoreId from AppUser if set, otherwise fall back to first store
        var defaultStoreId = appUser.DefaultStoreId ?? stores.FirstOrDefault()?.Id;

        // Return updated profile
        var address = appUser.Address;
        return new AppUserProfileResponseDto
        {
            Username = userAccount.Username,
            Email = appUser.Email,
            Language = appUser.Language,
            Phone = appUser.PhoneNumber,
            Mobile = appUser.PhoneNumber,
            StreetNumber = address?.StreetNumber,
            StreetName = address?.StreetName,
            City = address?.City,
            State = address?.State,
            Country = appUser.Country,
            PostalCode = address?.PostalCode,
            DefaultStoreId = defaultStoreId,
            EnableTwoFactor = userAccount.TwoFactorEnabled
        };
    }

    public async Task<AppUserProfileResponseDto> GetAppUserProfileByUserAccountIdAsync(Guid userAccountId)
    {
        // Get UserAccount by ID
        var userAccount = await _userAccountRepository.GetByIdAsync(userAccountId);
        if (userAccount == null)
            throw new EntityNotFoundException("UserAccount", userAccountId);

        // Check if user is AppUser
        if (userAccount.UserType != UserType.AppUser)
            throw new System.InvalidOperationException($"User with ID {userAccountId} is not an AppUser. Current type: {userAccount.UserType}");

        if (!userAccount.AppUserId.HasValue)
            throw new System.InvalidOperationException($"AppUserId not found for UserAccount {userAccountId}");

        // Get AppUser profile using AppUserId
        return await GetAppUserProfileAsync(userAccount.AppUserId.Value);
    }

    public async Task<AppUserProfileResponseDto> CreateOrUpdateAppUserProfileByUserAccountIdAsync(Guid userAccountId, AppUserProfileRequestDto request)
    {
        // Get UserAccount by ID
        var userAccount = await _userAccountRepository.GetByIdAsync(userAccountId);
        if (userAccount == null)
            throw new EntityNotFoundException("UserAccount", userAccountId);

        // Check if user is AppUser
        if (userAccount.UserType != UserType.AppUser)
            throw new System.InvalidOperationException($"User with ID {userAccountId} is not an AppUser. Current type: {userAccount.UserType}");

        if (!userAccount.AppUserId.HasValue)
            throw new System.InvalidOperationException($"AppUserId not found for UserAccount {userAccountId}");

        // Create or update AppUser profile using AppUserId
        return await CreateOrUpdateAppUserProfileAsync(userAccount.AppUserId.Value, request);
    }

    private (string hash, string salt) HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var saltBytes = new byte[16];
        rng.GetBytes(saltBytes);
        var salt = Convert.ToBase64String(saltBytes);

        using var sha256 = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(password + salt);
        var hashBytes = sha256.ComputeHash(passwordBytes);
        var hash = Convert.ToBase64String(hashBytes);

        return (hash, salt);
    }

    private string HashAccessPin(string accessPin)
    {
        using var sha256 = SHA256.Create();
        var pinBytes = Encoding.UTF8.GetBytes(accessPin);
        var hashBytes = sha256.ComputeHash(pinBytes);
        return Convert.ToBase64String(hashBytes);
    }
}

