using Flowtap_Domain.DtoModel;

namespace Flowtap_Application.Interfaces;

public interface ICustomerService
{
    Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerRequestDto request);
    Task<CustomerResponseDto> GetCustomerByIdAsync(Guid id);
    Task<IEnumerable<CustomerResponseDto>> GetAllCustomersAsync();
    Task<IEnumerable<CustomerResponseDto>> SearchCustomersAsync(string searchTerm);
    Task<IEnumerable<CustomerResponseDto>> GetCustomersByStatusAsync(string status);
    Task<CustomerResponseDto> UpdateCustomerAsync(Guid id, UpdateCustomerRequestDto request);
    Task<bool> DeleteCustomerAsync(Guid id);
    Task<CustomerResponseDto> MarkCustomerAsVipAsync(Guid id);
}

