using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Presentation.Attributes;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Device Categories Controller - Manage device categories
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AppUserOrOwner]
public class DeviceCategoriesController : ControllerBase
{
    private readonly IDeviceCategoryService _deviceCategoryService;
    private readonly ILogger<DeviceCategoriesController> _logger;

    public DeviceCategoriesController(
        IDeviceCategoryService deviceCategoryService,
        ILogger<DeviceCategoriesController> logger)
    {
        _deviceCategoryService = deviceCategoryService;
        _logger = logger;
    }

    /// <summary>
    /// Get all device categories
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<DeviceCategoryResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<DeviceCategoryResponseDto>>>> GetAllDeviceCategories()
    {
        try
        {
            var categories = await _deviceCategoryService.GetAllDeviceCategoriesAsync();
            return Ok(ApiResponseDto<IEnumerable<DeviceCategoryResponseDto>>.Success(categories, "Device categories retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving device categories");
            return BadRequest(ApiResponseDto<IEnumerable<DeviceCategoryResponseDto>>.Failure("Error retrieving device categories", null));
        }
    }

    /// <summary>
    /// Get device category by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<DeviceCategoryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<DeviceCategoryResponseDto>>> GetDeviceCategoryById(Guid id)
    {
        try
        {
            var category = await _deviceCategoryService.GetDeviceCategoryByIdAsync(id);
            return Ok(ApiResponseDto<DeviceCategoryResponseDto>.Success(category, "Device category retrieved successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<DeviceCategoryResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving device category {CategoryId}", id);
            return BadRequest(ApiResponseDto<DeviceCategoryResponseDto>.Failure("Error retrieving device category", null));
        }
    }

    /// <summary>
    /// Create a new device category
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<DeviceCategoryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponseDto<DeviceCategoryResponseDto>>> CreateDeviceCategory([FromBody] CreateDeviceCategoryRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<DeviceCategoryResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var category = await _deviceCategoryService.CreateDeviceCategoryAsync(request);
            return Ok(ApiResponseDto<DeviceCategoryResponseDto>.Success(category, "Device category created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating device category");
            return BadRequest(ApiResponseDto<DeviceCategoryResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Update device category
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<DeviceCategoryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<DeviceCategoryResponseDto>>> UpdateDeviceCategory(Guid id, [FromBody] UpdateDeviceCategoryRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<DeviceCategoryResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var category = await _deviceCategoryService.UpdateDeviceCategoryAsync(id, request);
            return Ok(ApiResponseDto<DeviceCategoryResponseDto>.Success(category, "Device category updated successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<DeviceCategoryResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating device category {CategoryId}", id);
            return BadRequest(ApiResponseDto<DeviceCategoryResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Delete device category
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteDeviceCategory(Guid id)
    {
        try
        {
            var result = await _deviceCategoryService.DeleteDeviceCategoryAsync(id);
            return Ok(ApiResponseDto<bool>.Success(result, "Device category deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting device category {CategoryId}", id);
            return BadRequest(ApiResponseDto<bool>.Failure("Error deleting device category", false));
        }
    }
}

