using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeResponseDto> CreateEmployeeAsync(CreateEmployeeRequestDto request);
    Task<EmployeeResponseDto> GetEmployeeByIdAsync(Guid id);
    Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync();
    Task<IEnumerable<EmployeeResponseDto>> GetEmployeesByStoreIdAsync(Guid storeId);
    Task<IEnumerable<EmployeeResponseDto>> GetEmployeesByRoleAsync(string role);
    Task<IEnumerable<EmployeeResponseDto>> GetActiveEmployeesAsync();
    Task<EmployeeResponseDto> UpdateEmployeeAsync(Guid id, UpdateEmployeeRequestDto request);
    Task<EmployeeResponseDto> ActivateEmployeeAsync(Guid id);
    Task<EmployeeResponseDto> DeactivateEmployeeAsync(Guid id);
    Task<bool> DeleteEmployeeAsync(Guid id);
    Task<EmployeeResponseDto> AddPartnerAsync(Guid ownerAppUserId, AddPartnerRequestDto request);
}

