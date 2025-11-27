using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface IServiceService
{
    Task<ServiceResponseDto> CreateServiceAsync(CreateServiceRequestDto request);
    Task<ServiceResponseDto> GetServiceByIdAsync(Guid id);
    Task<IEnumerable<ServiceResponseDto>> GetServicesByStoreIdAsync(Guid storeId);
    Task<IEnumerable<ServiceResponseDto>> GetActiveServicesAsync(Guid storeId);
    Task<ServiceResponseDto> UpdateServiceAsync(Guid id, UpdateServiceRequestDto request);
    Task<bool> DeleteServiceAsync(Guid id);
}

