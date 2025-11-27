using AutoMapper;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.BoundedContexts.Service.Interfaces;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class DeviceCategoryService : IDeviceCategoryService
{
    private readonly IDeviceCategoryRepository _deviceCategoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeviceCategoryService> _logger;

    public DeviceCategoryService(
        IDeviceCategoryRepository deviceCategoryRepository,
        IMapper mapper,
        ILogger<DeviceCategoryService> logger)
    {
        _deviceCategoryRepository = deviceCategoryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<DeviceCategoryResponseDto> CreateDeviceCategoryAsync(CreateDeviceCategoryRequestDto request)
    {
        var existing = await _deviceCategoryRepository.GetByNameAsync(request.Name);
        if (existing != null)
        {
            throw new System.InvalidOperationException($"Device category with name '{request.Name}' already exists");
        }

        var category = _mapper.Map<DeviceCategory>(request);
        category.Id = Guid.NewGuid();

        var created = await _deviceCategoryRepository.CreateAsync(category);
        _logger.LogInformation("Created device category {CategoryId}", created.Id);

        return await MapToResponseDto(created);
    }

    public async Task<DeviceCategoryResponseDto> GetDeviceCategoryByIdAsync(Guid id)
    {
        var category = await _deviceCategoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            throw new EntityNotFoundException("DeviceCategory", id);
        }

        return await MapToResponseDto(category);
    }

    public async Task<IEnumerable<DeviceCategoryResponseDto>> GetAllDeviceCategoriesAsync()
    {
        var categories = await _deviceCategoryRepository.GetAllAsync();
        var result = new List<DeviceCategoryResponseDto>();

        foreach (var category in categories)
        {
            result.Add(await MapToResponseDto(category));
        }

        return result;
    }

    public async Task<DeviceCategoryResponseDto> UpdateDeviceCategoryAsync(Guid id, UpdateDeviceCategoryRequestDto request)
    {
        var category = await _deviceCategoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            throw new EntityNotFoundException("DeviceCategory", id);
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
            category.UpdateName(request.Name);
        if (request.PictureUrl != null)
            category.UpdatePictureUrl(request.PictureUrl);
        if (request.IsLaborBillable.HasValue)
            category.SetLaborBillable(request.IsLaborBillable.Value);

        await _deviceCategoryRepository.UpdateAsync(category);

        return await MapToResponseDto(category);
    }

    public async Task<bool> DeleteDeviceCategoryAsync(Guid id)
    {
        return await _deviceCategoryRepository.DeleteAsync(id);
    }

    private async Task<DeviceCategoryResponseDto> MapToResponseDto(DeviceCategory category)
    {
        var dto = _mapper.Map<DeviceCategoryResponseDto>(category);
        dto.BrandCount = category.Brands?.Count ?? 0;
        dto.PictureUrl = category.PictureUrl;
        dto.IsLaborBillable = category.IsLaborBillable;
        return dto;
    }
}

