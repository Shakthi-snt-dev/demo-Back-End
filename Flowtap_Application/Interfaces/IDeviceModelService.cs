using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface IDeviceModelService
{
    Task<DeviceModelResponseDto> CreateDeviceModelAsync(CreateDeviceModelRequestDto request);
    Task<DeviceModelResponseDto> GetDeviceModelByIdAsync(Guid id);
    Task<IEnumerable<DeviceModelResponseDto>> GetDeviceModelsByBrandIdAsync(Guid brandId);
    Task<DeviceModelResponseDto> UpdateDeviceModelAsync(Guid id, CreateDeviceModelRequestDto request);
    Task<bool> DeleteDeviceModelAsync(Guid id);
}

