using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

/// <summary>
/// Store Settings Service Interface - Manage store settings
/// </summary>
public interface IStoreSettingsService
{
    // Internal helper methods (used internally by public methods)
    Task<StoreSettingsDto> GetStoreSettingsAsync(Guid storeId);
    Task<StoreSettingsDto> UpdateStoreSettingsAsync(Guid storeId, UpdateStoreSettingsRequestDto request);
    Task<string> ResetApiKeyAsync(Guid storeId);
    
    // Public API methods (used by controllers) - with specific store
    Task<StoreSettingsDto> GetStoreSettingsByUserAccountIdAsync(Guid userAccountId, Guid storeId);
    Task<StoreSettingsDto> UpdateStoreSettingsByUserAccountIdAsync(Guid userAccountId, Guid storeId, UpdateStoreSettingsRequestDto request);
    Task<string> ResetApiKeyByUserAccountIdAsync(Guid userAccountId, Guid storeId);
    
    
    // Company email verification methods
    Task<bool> VerifyCompanyEmailAsync(Guid userAccountId, Guid storeId, string verificationToken);
    Task<bool> SendCompanyEmailVerificationAsync(Guid userAccountId, Guid storeId);
}

