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
}

