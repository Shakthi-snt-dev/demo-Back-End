using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface IDeviceCategoryService
{
    Task<DeviceCategoryResponseDto> CreateDeviceCategoryAsync(CreateDeviceCategoryRequestDto request);
    Task<DeviceCategoryResponseDto> GetDeviceCategoryByIdAsync(Guid id);
    Task<IEnumerable<DeviceCategoryResponseDto>> GetAllDeviceCategoriesAsync();
    Task<DeviceCategoryResponseDto> UpdateDeviceCategoryAsync(Guid id, UpdateDeviceCategoryRequestDto request);
    Task<bool> DeleteDeviceCategoryAsync(Guid id);
}

