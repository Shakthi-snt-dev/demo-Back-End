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

public class SettingsService : ISettingsService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IAppUserAdminRepository _appUserAdminRepository;
    private readonly ILogger<SettingsService> _logger;

    public SettingsService(
        IAppUserRepository appUserRepository,
        IStoreRepository storeRepository,
        IUserAccountRepository userAccountRepository,
        IEmployeeRepository employeeRepository,
        IAppUserAdminRepository appUserAdminRepository,
        ILogger<SettingsService> logger)
    {
        _appUserRepository = appUserRepository;
        _storeRepository = storeRepository;
        _userAccountRepository = userAccountRepository;
        _employeeRepository = employeeRepository;
        _appUserAdminRepository = appUserAdminRepository;
        _logger = logger;
    }

    public async Task<SettingsResponseDto> GetSettingsAsync(Guid appUserId)
    {
        var appUser = await _appUserRepository.GetByIdAsync(appUserId);
        if (appUser == null)
            throw new EntityNotFoundException("AppUser", appUserId);

        // Get first store for store-specific settings
        var stores = await _storeRepository.GetByAppUserIdAsync(appUserId);
        var firstStore = stores.FirstOrDefault();

        var userAccount = await _userAccountRepository.GetByEmailAsync(appUser.Email);

        return new SettingsResponseDto
        {
            General = new GeneralSettingsDto
            {
                Email = appUser.Email,
                TimeZone = appUser.TimeZone,
                Currency = appUser.Currency
            },
            Inventory = firstStore?.Settings != null
                ? new InventorySettingsDto
                {
                    EnableInventoryTracking = firstStore.Settings.EnableInventory,
                    // Map other settings from StoreSettings
                }
                : new InventorySettingsDto(),
            Notifications = new NotificationSettingsDto(),
            Payment = new PaymentSettingsDto(),
            Security = new SecuritySettingsDto
            {
                TwoFactorEnabled = userAccount?.TwoFactorEnabled ?? false
            }
        };
    }

    public async Task<GeneralSettingsDto> UpdateGeneralSettingsAsync(Guid appUserId, UpdateGeneralSettingsRequestDto request)
    {
        var appUser = await _appUserRepository.GetByIdAsync(appUserId);
        if (appUser == null)
            throw new EntityNotFoundException("AppUser", appUserId);

        if (!string.IsNullOrWhiteSpace(request.TimeZone))
            appUser.TimeZone = request.TimeZone;

        if (!string.IsNullOrWhiteSpace(request.Currency))
            appUser.Currency = request.Currency;

        await _appUserRepository.UpdateAsync(appUser);

        return new GeneralSettingsDto
        {
            Email = appUser.Email,
            TimeZone = appUser.TimeZone,
            Currency = appUser.Currency
        };
    }

    public async Task<InventorySettingsDto> UpdateInventorySettingsAsync(Guid storeId, UpdateInventorySettingsRequestDto request)
    {
        var store = await _storeRepository.GetByIdAsync(storeId);
        if (store == null)
            throw new EntityNotFoundException("Store", storeId);

        if (request.EnableInventoryTracking.HasValue)
            store.Settings.EnableInventory = request.EnableInventoryTracking.Value;

        await _storeRepository.UpdateAsync(store);

        return new InventorySettingsDto
        {
            EnableInventoryTracking = store.Settings.EnableInventory
        };
    }

    public async Task<NotificationSettingsDto> UpdateNotificationSettingsAsync(Guid appUserId, UpdateNotificationSettingsRequestDto request)
    {
        // Notification settings would typically be stored in AppUser or a separate settings entity
        // For now, return default settings
        return new NotificationSettingsDto();
    }

    public async Task<PaymentSettingsDto> UpdatePaymentSettingsAsync(Guid storeId, UpdatePaymentSettingsRequestDto request)
    {
        // Payment settings would typically be stored in StoreSettings
        // For now, return default settings
        return new PaymentSettingsDto();
    }

    public async Task<bool> UpdatePasswordAsync(Guid appUserId, UpdateSecuritySettingsRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.NewPassword) || string.IsNullOrWhiteSpace(request.CurrentPassword))
            throw new ArgumentException("Current password and new password are required");

        if (request.NewPassword != request.ConfirmPassword)
            throw new ArgumentException("New password and confirm password do not match");

        var appUser = await _appUserRepository.GetByIdAsync(appUserId);
        if (appUser == null)
            throw new EntityNotFoundException("AppUser", appUserId);

        var userAccount = await _userAccountRepository.GetByEmailAsync(appUser.Email);
        if (userAccount == null)
            throw new EntityNotFoundException("UserAccount", Guid.Empty);

        // Validate current password
        using var sha256 = SHA256.Create();
        var currentPasswordBytes = Encoding.UTF8.GetBytes(request.CurrentPassword + (userAccount.PasswordSalt ?? ""));
        var currentHashBytes = sha256.ComputeHash(currentPasswordBytes);
        var currentHash = Convert.ToBase64String(currentHashBytes);

        if (currentHash != userAccount.PasswordHash)
            throw new UnauthorizedException("Current password is incorrect");

        // Hash new password
        var salt = Guid.NewGuid().ToString();
        var newPasswordBytes = Encoding.UTF8.GetBytes(request.NewPassword + salt);
        var newHashBytes = sha256.ComputeHash(newPasswordBytes);
        var newHash = Convert.ToBase64String(newHashBytes);

        userAccount.UpdatePassword(newHash, salt);
        await _userAccountRepository.UpdateAsync(userAccount);

        return true;
    }

    public async Task<bool> EnableTwoFactorAsync(Guid appUserId)
    {
        var appUser = await _appUserRepository.GetByIdAsync(appUserId);
        if (appUser == null)
            throw new EntityNotFoundException("AppUser", appUserId);

        var userAccount = await _userAccountRepository.GetByEmailAsync(appUser.Email);
        if (userAccount == null)
            throw new EntityNotFoundException("UserAccount", Guid.Empty);

        userAccount.EnableTwoFactor();
        await _userAccountRepository.UpdateAsync(userAccount);

        return true;
    }

    public async Task<UserProfileDto> GetUserProfileAsync(Guid appUserId)
    {
        var appUser = await _appUserRepository.GetByIdAsync(appUserId);
        if (appUser == null)
            throw new EntityNotFoundException("AppUser", appUserId);

        var userAccount = await _userAccountRepository.GetByEmailAsync(appUser.Email);
        var stores = await _storeRepository.GetByAppUserIdAsync(appUserId);
        var defaultStoreId = stores.FirstOrDefault()?.Id;

        return new UserProfileDto
        {
            Username = userAccount?.Username,
            Email = appUser.Email,
            Phone = appUser.PhoneNumber,
            Mobile = appUser.PhoneNumber, // Using PhoneNumber as mobile if separate field not available
            Address = appUser.Address,
            DefaultStoreId = defaultStoreId,
            EnableTwoFactor = userAccount?.TwoFactorEnabled ?? false,
            UserType = userAccount?.UserType ?? Flowtap_Domain.SharedKernel.Enums.UserType.AppUser
        };
    }

    public async Task<UserProfileDto> UpdateUserProfileAsync(Guid appUserId, UpdateUserProfileRequestDto request)
    {
        var appUser = await _appUserRepository.GetByIdAsync(appUserId);
        if (appUser == null)
            throw new EntityNotFoundException("AppUser", appUserId);

        var userAccount = await _userAccountRepository.GetByEmailAsync(appUser.Email);
        if (userAccount == null)
            throw new EntityNotFoundException("UserAccount", Guid.Empty);

        // Update AppUser fields
        if (!string.IsNullOrWhiteSpace(request.Email))
            appUser.Email = request.Email;

        if (!string.IsNullOrWhiteSpace(request.Phone))
            appUser.PhoneNumber = request.Phone;

        if (request.Address != null)
            appUser.Address = request.Address;

        await _appUserRepository.UpdateAsync(appUser);

        // Update UserAccount fields
        if (!string.IsNullOrWhiteSpace(request.Username))
            userAccount.Username = request.Username;

        if (request.UserType.HasValue)
            userAccount.UserType = request.UserType.Value;

        if (request.EnableTwoFactor.HasValue && request.EnableTwoFactor.Value)
            userAccount.EnableTwoFactor();

        await _userAccountRepository.UpdateAsync(userAccount);

        return await GetUserProfileAsync(appUserId);
    }

    public async Task<StoreSettingsDto> GetStoreSettingsAsync(Guid storeId)
    {
        var store = await _storeRepository.GetByIdAsync(storeId);
        if (store == null)
            throw new EntityNotFoundException("Store", storeId);

        return new StoreSettingsDto
        {
            BusinessName = store.StoreName,
            Phone = store.Phone,
            Address = store.Address,
            TimeZone = store.Settings?.TimeZone ?? "UTC",
            // Map other settings from StoreSettings entity
            // Note: Many fields may need to be stored in StoreSettings or a separate configuration
        };
    }

    public async Task<StoreSettingsDto> UpdateStoreSettingsAsync(Guid storeId, UpdateStoreSettingsRequestDto request)
    {
        var store = await _storeRepository.GetByIdAsync(storeId);
        if (store == null)
            throw new EntityNotFoundException("Store", storeId);

        // Update basic store fields
        if (!string.IsNullOrWhiteSpace(request.BusinessName))
            store.StoreName = request.BusinessName;

        if (!string.IsNullOrWhiteSpace(request.Phone))
            store.Phone = request.Phone;

        if (request.Address != null)
            store.UpdateAddress(request.Address);

        // Update store settings
        if (!string.IsNullOrWhiteSpace(request.TimeZone))
            store.UpdateSettings(
                store.Settings?.EnablePOS ?? true,
                store.Settings?.EnableInventory ?? true,
                request.TimeZone);

        await _storeRepository.UpdateAsync(store);

        return await GetStoreSettingsAsync(storeId);
    }

    public async Task<string> ResetApiKeyAsync(Guid storeId)
    {
        var store = await _storeRepository.GetByIdAsync(storeId);
        if (store == null)
            throw new EntityNotFoundException("Store", storeId);

        // Generate new API key
        var newApiKey = Guid.NewGuid().ToString("N");
        // TODO: Store API key in StoreSettings or a separate entity
        // For now, return generated key
        return newApiKey;
    }

    public async Task<CheckUserTypeResponseDto> CheckUserTypeByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));

        var response = new CheckUserTypeResponseDto
        {
            Exists = false
        };

        // Check UserAccount first
        var userAccount = await _userAccountRepository.GetByEmailAsync(email);
        if (userAccount == null)
        {
            return response; // User doesn't exist
        }

        response.Exists = true;

        // Determine user type based on UserAccount.UserType
        switch (userAccount.UserType)
        {
            case UserType.AppUser:
                response.UserType = "AppUser";
                response.AppUserId = userAccount.AppUserId;
                break;

            case UserType.Employee:
                response.UserType = "Employee";
                response.EmployeeId = userAccount.EmployeeId;
                // Also check if linked to AppUser
                if (userAccount.AppUserId.HasValue)
                {
                    response.AppUserId = userAccount.AppUserId;
                }
                break;

            // Admin user type removed - AppUserAdmin is no longer in DbContext
            default:
                response.UserType = userAccount.UserType.ToString();
                break;
        }

        return response;
    }

    public async Task<UpdateAppUserProfileResponseDto> UpdateAppUserProfileAsync(string email, UpdateAppUserProfileRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));

        // Check user type first
        var userTypeResponse = await CheckUserTypeByEmailAsync(email);
        if (!userTypeResponse.Exists)
            throw new EntityNotFoundException("User", "Email", email);

        if (userTypeResponse.UserType != "AppUser")
            throw new System.InvalidOperationException($"User with email {email} is not an AppUser. Current type: {userTypeResponse.UserType}");

        if (!userTypeResponse.AppUserId.HasValue)
            throw new System.InvalidOperationException($"AppUserId not found for email {email}");

        var appUserId = userTypeResponse.AppUserId.Value;

        // Get AppUser
        var appUser = await _appUserRepository.GetByIdAsync(appUserId);
        if (appUser == null)
            throw new EntityNotFoundException("AppUser", appUserId);

        // Get UserAccount
        var userAccount = await _userAccountRepository.GetByEmailAsync(email);
        if (userAccount == null)
            throw new EntityNotFoundException("UserAccount", "Email", email);

        // Update FirstName
        if (!string.IsNullOrWhiteSpace(request.FirstName))
        {
            appUser.FirstName = request.FirstName;
        }

        // Update LastName
        if (!string.IsNullOrWhiteSpace(request.LastName))
        {
            appUser.LastName = request.LastName;
        }

        // Update Address
        if (request.Address != null)
        {
            appUser.UpdateAddress(request.Address);
        }

        // Update Password if provided
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            var (passwordHash, passwordSalt) = HashPassword(request.Password);
            userAccount.UpdatePassword(passwordHash, passwordSalt);
        }

        // Save AppUser changes
        await _appUserRepository.UpdateAsync(appUser);

        // Save UserAccount changes
        await _userAccountRepository.UpdateAsync(userAccount);

        // Get stores for this AppUser
        var stores = await _storeRepository.GetByAppUserIdAsync(appUserId);
        var firstStore = stores.FirstOrDefault();

        Guid? employeeId = null;

        // Create or update Employee with Owner role and AccessPin
        if (firstStore != null)
        {
            // Check if employee already exists for this AppUser and store
            var existingEmployees = await _employeeRepository.GetByLinkedAppUserIdAsync(appUserId);
            var existingEmployee = existingEmployees.FirstOrDefault(e => e.StoreId == firstStore.Id);

            if (existingEmployee != null)
            {
                // Update existing employee
                employeeId = existingEmployee.Id;

                // Update FullName
                var fullName = $"{appUser.FirstName} {appUser.LastName}".Trim();
                if (!string.IsNullOrWhiteSpace(fullName))
                {
                    existingEmployee.FullName = fullName;
                }

                // Update Address if provided
                if (request.Address != null)
                {
                    existingEmployee.UpdateAddress(request.Address);
                }

                // Update AccessPin if provided
                if (!string.IsNullOrWhiteSpace(request.AccessPin))
                {
                    existingEmployee.AccessPinHash = HashAccessPin(request.AccessPin);
                }

                // Ensure role is Owner
                if (existingEmployee.Role != EmployeeRole.Owner.ToString())
                {
                    existingEmployee.UpdateRole(EmployeeRole.Owner.ToString());
                }

                await _employeeRepository.UpdateAsync(existingEmployee);
            }
            else
            {
                // Create new employee
                var employee = new Employee
                {
                    Id = Guid.NewGuid(),
                    StoreId = firstStore.Id,
                    FullName = $"{appUser.FirstName} {appUser.LastName}".Trim(),
                    Email = appUser.Email,
                    Role = EmployeeRole.Owner.ToString(),
                    LinkedAppUserId = appUserId,
                    IsActive = true,
                    CanSwitchRole = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Set AccessPin if provided
                if (!string.IsNullOrWhiteSpace(request.AccessPin))
                {
                    employee.AccessPinHash = HashAccessPin(request.AccessPin);
                }

                // Set Address if provided
                if (request.Address != null)
                {
                    employee.Address = request.Address;
                }

                var createdEmployee = await _employeeRepository.AddAsync(employee);
                employeeId = createdEmployee.Id;

                // Update UserAccount with EmployeeId
                userAccount.EmployeeId = createdEmployee.Id;
                await _userAccountRepository.UpdateAsync(userAccount);
            }
        }
        else
        {
            _logger.LogWarning("No store found for AppUser {AppUserId}. Employee record not created.", appUserId);
        }

        return new UpdateAppUserProfileResponseDto
        {
            AppUserId = appUserId,
            Email = appUser.Email,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            EmployeeId = employeeId,
            Message = "Profile updated successfully. Employee record created/updated with Owner role."
        };
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
        var defaultStoreId = stores.FirstOrDefault()?.Id;

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

        // Save changes
        await _appUserRepository.UpdateAsync(appUser);
        await _userAccountRepository.UpdateAsync(userAccount);

        // Get updated stores
        var stores = await _storeRepository.GetByAppUserIdAsync(appUserId);
        var defaultStoreId = request.DefaultStoreId ?? stores.FirstOrDefault()?.Id;

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
}

