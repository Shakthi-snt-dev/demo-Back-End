using AutoMapper;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.BoundedContexts.Service.Interfaces;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class DeviceBrandService : IDeviceBrandService
{
    private readonly IDeviceBrandRepository _deviceBrandRepository;
    private readonly IDeviceCategoryRepository _deviceCategoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeviceBrandService> _logger;

    public DeviceBrandService(
        IDeviceBrandRepository deviceBrandRepository,
        IDeviceCategoryRepository deviceCategoryRepository,
        IMapper mapper,
        ILogger<DeviceBrandService> logger)
    {
        _deviceBrandRepository = deviceBrandRepository;
        _deviceCategoryRepository = deviceCategoryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<DeviceBrandResponseDto> CreateDeviceBrandAsync(CreateDeviceBrandRequestDto request)
    {
        var category = await _deviceCategoryRepository.GetByIdAsync(request.CategoryId);
        if (category == null)
        {
            throw new EntityNotFoundException("DeviceCategory", request.CategoryId);
        }

        var brand = _mapper.Map<DeviceBrand>(request);
        brand.Id = Guid.NewGuid();

        var created = await _deviceBrandRepository.CreateAsync(brand);
        _logger.LogInformation("Created device brand {BrandId} for category {CategoryId}", created.Id, request.CategoryId);

        return await MapToResponseDto(created);
    }

    public async Task<DeviceBrandResponseDto> GetDeviceBrandByIdAsync(Guid id)
    {
        var brand = await _deviceBrandRepository.GetByIdAsync(id);
        if (brand == null)
        {
            throw new EntityNotFoundException("DeviceBrand", id);
        }

        return await MapToResponseDto(brand);
    }

    public async Task<IEnumerable<DeviceBrandResponseDto>> GetDeviceBrandsByCategoryIdAsync(Guid categoryId)
    {
        var brands = await _deviceBrandRepository.GetByCategoryIdAsync(categoryId);
        var result = new List<DeviceBrandResponseDto>();

        foreach (var brand in brands)
        {
            result.Add(await MapToResponseDto(brand));
        }

        return result;
    }

    public async Task<DeviceBrandResponseDto> UpdateDeviceBrandAsync(Guid id, CreateDeviceBrandRequestDto request)
    {
        var brand = await _deviceBrandRepository.GetByIdAsync(id);
        if (brand == null)
        {
            throw new EntityNotFoundException("DeviceBrand", id);
        }

        brand.UpdateName(request.Name);
        if (brand.CategoryId != request.CategoryId)
        {
            brand.ChangeCategory(request.CategoryId);
        }

        await _deviceBrandRepository.UpdateAsync(brand);

        return await MapToResponseDto(brand);
    }

    public async Task<bool> DeleteDeviceBrandAsync(Guid id)
    {
        return await _deviceBrandRepository.DeleteAsync(id);
    }

    private async Task<DeviceBrandResponseDto> MapToResponseDto(DeviceBrand brand)
    {
        var dto = _mapper.Map<DeviceBrandResponseDto>(brand);
        if (brand.Category != null)
        {
            dto.CategoryName = brand.Category.Name;
        }
        dto.ModelCount = brand.Models?.Count ?? 0;
        return dto;
    }
}

