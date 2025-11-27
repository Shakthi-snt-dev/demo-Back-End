using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface IDeviceBrandService
{
    Task<DeviceBrandResponseDto> CreateDeviceBrandAsync(CreateDeviceBrandRequestDto request);
    Task<DeviceBrandResponseDto> GetDeviceBrandByIdAsync(Guid id);
    Task<IEnumerable<DeviceBrandResponseDto>> GetDeviceBrandsByCategoryIdAsync(Guid categoryId);
    Task<DeviceBrandResponseDto> UpdateDeviceBrandAsync(Guid id, CreateDeviceBrandRequestDto request);
    Task<bool> DeleteDeviceBrandAsync(Guid id);
}

