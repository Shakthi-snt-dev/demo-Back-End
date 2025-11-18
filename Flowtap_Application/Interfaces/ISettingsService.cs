using Flowtap_Domain.DtoModel;

namespace Flowtap_Application.Interfaces;

public interface ISettingsService
{
    Task<SettingsResponseDto> GetSettingsAsync(Guid appUserId);
    Task<GeneralSettingsDto> UpdateGeneralSettingsAsync(Guid appUserId, UpdateGeneralSettingsRequestDto request);
    Task<InventorySettingsDto> UpdateInventorySettingsAsync(Guid storeId, UpdateInventorySettingsRequestDto request);
    Task<NotificationSettingsDto> UpdateNotificationSettingsAsync(Guid appUserId, UpdateNotificationSettingsRequestDto request);
    Task<PaymentSettingsDto> UpdatePaymentSettingsAsync(Guid storeId, UpdatePaymentSettingsRequestDto request);
    Task<bool> UpdatePasswordAsync(Guid appUserId, UpdateSecuritySettingsRequestDto request);
    Task<bool> EnableTwoFactorAsync(Guid appUserId);
    Task<UserProfileDto> GetUserProfileAsync(Guid appUserId);
    Task<UserProfileDto> UpdateUserProfileAsync(Guid appUserId, UpdateUserProfileRequestDto request);
    Task<StoreSettingsDto> GetStoreSettingsAsync(Guid storeId);
    Task<StoreSettingsDto> UpdateStoreSettingsAsync(Guid storeId, UpdateStoreSettingsRequestDto request);
    Task<string> ResetApiKeyAsync(Guid storeId);
    Task<CheckUserTypeResponseDto> CheckUserTypeByEmailAsync(string email);
    Task<UpdateAppUserProfileResponseDto> UpdateAppUserProfileAsync(string email, UpdateAppUserProfileRequestDto request);
    Task<AppUserProfileResponseDto> GetAppUserProfileAsync(Guid appUserId);
    Task<AppUserProfileResponseDto> CreateOrUpdateAppUserProfileAsync(Guid appUserId, AppUserProfileRequestDto request);
    Task<AppUserProfileResponseDto> GetAppUserProfileByUserAccountIdAsync(Guid userAccountId);
    Task<AppUserProfileResponseDto> CreateOrUpdateAppUserProfileByUserAccountIdAsync(Guid userAccountId, AppUserProfileRequestDto request);
}

