using Flowtap_Domain.DtoModel;

namespace Flowtap_Application.Interfaces;

/// <summary>
/// Profile Service Interface - Manage user profile settings
/// </summary>
public interface IProfileService
{
    // Public API methods (used by controllers)
    Task<AppUserProfileResponseDto> GetAppUserProfileByUserAccountIdAsync(Guid userAccountId);
    Task<AppUserProfileResponseDto> CreateOrUpdateAppUserProfileByUserAccountIdAsync(Guid userAccountId, AppUserProfileRequestDto request);
    
    // Internal helper methods (used internally by public methods)
    Task<AppUserProfileResponseDto> GetAppUserProfileAsync(Guid appUserId);
    Task<AppUserProfileResponseDto> CreateOrUpdateAppUserProfileAsync(Guid appUserId, AppUserProfileRequestDto request);
}

