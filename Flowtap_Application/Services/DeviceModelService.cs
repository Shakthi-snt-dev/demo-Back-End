using AutoMapper;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.BoundedContexts.Service.Interfaces;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class DeviceModelService : IDeviceModelService
{
    private readonly IDeviceModelRepository _deviceModelRepository;
    private readonly IDeviceBrandRepository _deviceBrandRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeviceModelService> _logger;

    public DeviceModelService(
        IDeviceModelRepository deviceModelRepository,
        IDeviceBrandRepository deviceBrandRepository,
        IMapper mapper,
        ILogger<DeviceModelService> logger)
    {
        _deviceModelRepository = deviceModelRepository;
        _deviceBrandRepository = deviceBrandRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<DeviceModelResponseDto> CreateDeviceModelAsync(CreateDeviceModelRequestDto request)
    {
        var brand = await _deviceBrandRepository.GetByIdAsync(request.BrandId);
        if (brand == null)
        {
            throw new EntityNotFoundException("DeviceBrand", request.BrandId);
        }

        var model = _mapper.Map<DeviceModel>(request);
        model.Id = Guid.NewGuid();

        var created = await _deviceModelRepository.CreateAsync(model);
        _logger.LogInformation("Created device model {ModelId} for brand {BrandId}", created.Id, request.BrandId);

        return await MapToResponseDto(created);
    }

    public async Task<DeviceModelResponseDto> GetDeviceModelByIdAsync(Guid id)
    {
        var model = await _deviceModelRepository.GetByIdAsync(id);
        if (model == null)
        {
            throw new EntityNotFoundException("DeviceModel", id);
        }

        return await MapToResponseDto(model);
    }

    public async Task<IEnumerable<DeviceModelResponseDto>> GetDeviceModelsByBrandIdAsync(Guid brandId)
    {
        var models = await _deviceModelRepository.GetByBrandIdAsync(brandId);
        var result = new List<DeviceModelResponseDto>();

        foreach (var model in models)
        {
            result.Add(await MapToResponseDto(model));
        }

        return result;
    }

    public async Task<DeviceModelResponseDto> UpdateDeviceModelAsync(Guid id, CreateDeviceModelRequestDto request)
    {
        var model = await _deviceModelRepository.GetByIdAsync(id);
        if (model == null)
        {
            throw new EntityNotFoundException("DeviceModel", id);
        }

        model.UpdateName(request.Name);
        if (model.BrandId != request.BrandId)
        {
            model.ChangeBrand(request.BrandId);
        }

        await _deviceModelRepository.UpdateAsync(model);

        return await MapToResponseDto(model);
    }

    public async Task<bool> DeleteDeviceModelAsync(Guid id)
    {
        return await _deviceModelRepository.DeleteAsync(id);
    }

    private async Task<DeviceModelResponseDto> MapToResponseDto(DeviceModel model)
    {
        var dto = _mapper.Map<DeviceModelResponseDto>(model);
        if (model.Brand != null)
        {
            dto.BrandName = model.Brand.Name;
        }
        return dto;
    }
}

