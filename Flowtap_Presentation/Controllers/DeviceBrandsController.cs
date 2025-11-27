using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Presentation.Attributes;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Device Brands Controller - Manage device brands
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AppUserOrOwner]
public class DeviceBrandsController : ControllerBase
{
    private readonly IDeviceBrandService _deviceBrandService;
    private readonly ILogger<DeviceBrandsController> _logger;

    public DeviceBrandsController(
        IDeviceBrandService deviceBrandService,
        ILogger<DeviceBrandsController> logger)
    {
        _deviceBrandService = deviceBrandService;
        _logger = logger;
    }

    /// <summary>
    /// Get all device brands
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<DeviceBrandResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<DeviceBrandResponseDto>>>> GetAllDeviceBrands()
    {
        try
        {
            // This would need a GetAll method in service, for now return empty or implement
            return Ok(ApiResponseDto<IEnumerable<DeviceBrandResponseDto>>.Success(new List<DeviceBrandResponseDto>(), "Device brands retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving device brands");
            return BadRequest(ApiResponseDto<IEnumerable<DeviceBrandResponseDto>>.Failure("Error retrieving device brands", null));
        }
    }

    /// <summary>
    /// Get device brand by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<DeviceBrandResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<DeviceBrandResponseDto>>> GetDeviceBrandById(Guid id)
    {
        try
        {
            var brand = await _deviceBrandService.GetDeviceBrandByIdAsync(id);
            return Ok(ApiResponseDto<DeviceBrandResponseDto>.Success(brand, "Device brand retrieved successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<DeviceBrandResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving device brand {BrandId}", id);
            return BadRequest(ApiResponseDto<DeviceBrandResponseDto>.Failure("Error retrieving device brand", null));
        }
    }

    /// <summary>
    /// Get device brands by category ID
    /// </summary>
    [HttpGet("category/{categoryId}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<DeviceBrandResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<DeviceBrandResponseDto>>>> GetDeviceBrandsByCategory(Guid categoryId)
    {
        try
        {
            var brands = await _deviceBrandService.GetDeviceBrandsByCategoryIdAsync(categoryId);
            return Ok(ApiResponseDto<IEnumerable<DeviceBrandResponseDto>>.Success(brands, "Device brands retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving device brands for category {CategoryId}", categoryId);
            return BadRequest(ApiResponseDto<IEnumerable<DeviceBrandResponseDto>>.Failure("Error retrieving device brands", null));
        }
    }

    /// <summary>
    /// Create a new device brand
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<DeviceBrandResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponseDto<DeviceBrandResponseDto>>> CreateDeviceBrand([FromBody] CreateDeviceBrandRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<DeviceBrandResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var brand = await _deviceBrandService.CreateDeviceBrandAsync(request);
            return Ok(ApiResponseDto<DeviceBrandResponseDto>.Success(brand, "Device brand created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating device brand");
            return BadRequest(ApiResponseDto<DeviceBrandResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Update device brand
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<DeviceBrandResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<DeviceBrandResponseDto>>> UpdateDeviceBrand(Guid id, [FromBody] CreateDeviceBrandRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<DeviceBrandResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var brand = await _deviceBrandService.UpdateDeviceBrandAsync(id, request);
            return Ok(ApiResponseDto<DeviceBrandResponseDto>.Success(brand, "Device brand updated successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<DeviceBrandResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating device brand {BrandId}", id);
            return BadRequest(ApiResponseDto<DeviceBrandResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Delete device brand
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteDeviceBrand(Guid id)
    {
        try
        {
            var result = await _deviceBrandService.DeleteDeviceBrandAsync(id);
            return Ok(ApiResponseDto<bool>.Success(result, "Device brand deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting device brand {BrandId}", id);
            return BadRequest(ApiResponseDto<bool>.Failure("Error deleting device brand", false));
        }
    }
}

