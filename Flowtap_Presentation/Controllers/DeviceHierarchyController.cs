using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Presentation.Attributes;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Device Hierarchy Controller - Provides cascading data for device repair management
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AppUserOrOwner]
public class DeviceHierarchyController : ControllerBase
{
    private readonly IDeviceCategoryService _deviceCategoryService;
    private readonly IDeviceBrandService _deviceBrandService;
    private readonly IDeviceModelService _deviceModelService;
    private readonly ILogger<DeviceHierarchyController> _logger;

    public DeviceHierarchyController(
        IDeviceCategoryService deviceCategoryService,
        IDeviceBrandService deviceBrandService,
        IDeviceModelService deviceModelService,
        ILogger<DeviceHierarchyController> logger)
    {
        _deviceCategoryService = deviceCategoryService;
        _deviceBrandService = deviceBrandService;
        _deviceModelService = deviceModelService;
        _logger = logger;
    }

    /// <summary>
    /// Get full device hierarchy (Category → Brand → Model → Problems)
    /// </summary>
    [HttpGet("full-hierarchy")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<DeviceCategoryResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<DeviceCategoryResponseDto>>>> GetFullHierarchy()
    {
        try
        {
            var categories = await _deviceCategoryService.GetAllDeviceCategoriesAsync();
            return Ok(ApiResponseDto<IEnumerable<DeviceCategoryResponseDto>>.Success(categories, "Device hierarchy retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving device hierarchy");
            return BadRequest(ApiResponseDto<IEnumerable<DeviceCategoryResponseDto>>.Failure("Error retrieving device hierarchy", null));
        }
    }

    /// <summary>
    /// Get manufacturers by category ID (cascading filter)
    /// </summary>
    [HttpGet("category/{categoryId}/manufacturers")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<DeviceBrandResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<DeviceBrandResponseDto>>>> GetManufacturersByCategory(Guid categoryId)
    {
        try
        {
            var brands = await _deviceBrandService.GetDeviceBrandsByCategoryIdAsync(categoryId);
            return Ok(ApiResponseDto<IEnumerable<DeviceBrandResponseDto>>.Success(brands, "Manufacturers retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving manufacturers for category {CategoryId}", categoryId);
            return BadRequest(ApiResponseDto<IEnumerable<DeviceBrandResponseDto>>.Failure("Error retrieving manufacturers", null));
        }
    }

    /// <summary>
    /// Get device models by brand ID (cascading filter)
    /// </summary>
    [HttpGet("brand/{brandId}/models")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<DeviceModelResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<DeviceModelResponseDto>>>> GetModelsByBrand(Guid brandId)
    {
        try
        {
            var models = await _deviceModelService.GetDeviceModelsByBrandIdAsync(brandId);
            return Ok(ApiResponseDto<IEnumerable<DeviceModelResponseDto>>.Success(models, "Device models retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving device models for brand {BrandId}", brandId);
            return BadRequest(ApiResponseDto<IEnumerable<DeviceModelResponseDto>>.Failure("Error retrieving device models", null));
        }
    }

    /// <summary>
    /// Get cascading data for ticket creation (Category → Manufacturer → Device → Problems)
    /// </summary>
    [HttpGet("ticket-creation-data")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> GetTicketCreationData()
    {
        try
        {
            var categories = await _deviceCategoryService.GetAllDeviceCategoriesAsync();
            
            var result = new
            {
                Categories = categories,
                Message = "Ticket creation data retrieved successfully"
            };

            return Ok(ApiResponseDto<object>.Success(result, "Ticket creation data retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving ticket creation data");
            return BadRequest(ApiResponseDto<object>.Failure("Error retrieving ticket creation data", null));
        }
    }
}

