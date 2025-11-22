using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface ICustomerService
{
    Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerRequestDto request);
    Task<CustomerResponseDto> GetCustomerByIdAsync(Guid id);
    Task<IEnumerable<CustomerResponseDto>> GetAllCustomersAsync();
    Task<IEnumerable<CustomerResponseDto>> GetCustomersByUserAccountIdAsync(Guid userAccountId);
    Task<IEnumerable<CustomerResponseDto>> GetCustomersByStoreIdAsync(Guid storeId);
    Task<IEnumerable<CustomerResponseDto>> SearchCustomersAsync(string searchTerm);
    Task<IEnumerable<CustomerResponseDto>> GetCustomersByStatusAsync(string status);
    Task<CustomerResponseDto> UpdateCustomerAsync(Guid id, UpdateCustomerRequestDto request);
    Task<bool> DeleteCustomerAsync(Guid id);
    Task<CustomerResponseDto> MarkCustomerAsVipAsync(Guid id);
}

