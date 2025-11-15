using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Owner.Interfaces;
using Flowtap_Domain.BoundedContexts.Store.Interfaces;
using Flowtap_Domain.BoundedContexts.Identity.Interfaces;
using Flowtap_Domain.DtoModel;
using Flowtap_Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace Flowtap_Application.Services;

public class SettingsService : ISettingsService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly ILogger<SettingsService> _logger;

    public SettingsService(
        IAppUserRepository appUserRepository,
        IStoreRepository storeRepository,
        IUserAccountRepository userAccountRepository,
        ILogger<SettingsService> logger)
    {
        _appUserRepository = appUserRepository;
        _storeRepository = storeRepository;
        _userAccountRepository = userAccountRepository;
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
}

