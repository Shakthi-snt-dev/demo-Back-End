using Flowtap_Domain.DtoModel;

namespace Flowtap_Application.Interfaces;

public interface IAppUserAdminService
{
    Task<AppUserAdminResponseDto> CreateAppUserAdminAsync(Guid appUserId, CreateAppUserAdminRequestDto request);
    Task<AppUserAdminResponseDto> GetAppUserAdminByIdAsync(Guid id);
    Task<IEnumerable<AppUserAdminResponseDto>> GetAppUserAdminsByAppUserIdAsync(Guid appUserId);
    Task<AppUserAdminResponseDto> UpdateAppUserAdminAsync(Guid id, CreateAppUserAdminRequestDto request);
    Task<bool> DeleteAppUserAdminAsync(Guid id);
}

