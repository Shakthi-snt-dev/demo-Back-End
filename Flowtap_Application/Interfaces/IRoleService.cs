using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface IRoleService
{
    Task<RoleResponseDto> CreateRoleAsync(CreateRoleRequestDto request);
    Task<RoleResponseDto> GetRoleByIdAsync(Guid id);
    Task<RoleResponseDto?> GetRoleByNameAsync(string name);
    Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync();
    Task<RoleResponseDto> UpdateRoleAsync(Guid id, UpdateRoleRequestDto request);
    Task<bool> DeleteRoleAsync(Guid id);
}

