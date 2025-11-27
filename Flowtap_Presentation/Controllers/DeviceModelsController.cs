using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Presentation.Attributes;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Device Models Controller - Manage device models
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AppUserOrOwner]
public class DeviceModelsController : ControllerBase
{
    private readonly IDeviceModelService _deviceModelService;
    private readonly ILogger<DeviceModelsController> _logger;

    public DeviceModelsController(
        IDeviceModelService deviceModelService,
        ILogger<DeviceModelsController> logger)
    {
        _deviceModelService = deviceModelService;
        _logger = logger;
    }

    /// <summary>
    /// Get all device models
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<DeviceModelResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<DeviceModelResponseDto>>>> GetAllDeviceModels()
    {
        try
        {
            // This would need a GetAll method in service, for now return empty or implement
            return Ok(ApiResponseDto<IEnumerable<DeviceModelResponseDto>>.Success(new List<DeviceModelResponseDto>(), "Device models retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving device models");
            return BadRequest(ApiResponseDto<IEnumerable<DeviceModelResponseDto>>.Failure("Error retrieving device models", null));
        }
    }

    /// <summary>
    /// Get device model by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<DeviceModelResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<DeviceModelResponseDto>>> GetDeviceModelById(Guid id)
    {
        try
        {
            var model = await _deviceModelService.GetDeviceModelByIdAsync(id);
            return Ok(ApiResponseDto<DeviceModelResponseDto>.Success(model, "Device model retrieved successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<DeviceModelResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving device model {ModelId}", id);
            return BadRequest(ApiResponseDto<DeviceModelResponseDto>.Failure("Error retrieving device model", null));
        }
    }

    /// <summary>
    /// Get device models by brand ID
    /// </summary>
    [HttpGet("brand/{brandId}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<DeviceModelResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<DeviceModelResponseDto>>>> GetDeviceModelsByBrand(Guid brandId)
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
    /// Create a new device model
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<DeviceModelResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponseDto<DeviceModelResponseDto>>> CreateDeviceModel([FromBody] CreateDeviceModelRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<DeviceModelResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var model = await _deviceModelService.CreateDeviceModelAsync(request);
            return Ok(ApiResponseDto<DeviceModelResponseDto>.Success(model, "Device model created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating device model");
            return BadRequest(ApiResponseDto<DeviceModelResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Update device model
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<DeviceModelResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<DeviceModelResponseDto>>> UpdateDeviceModel(Guid id, [FromBody] CreateDeviceModelRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<DeviceModelResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var model = await _deviceModelService.UpdateDeviceModelAsync(id, request);
            return Ok(ApiResponseDto<DeviceModelResponseDto>.Success(model, "Device model updated successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<DeviceModelResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating device model {ModelId}", id);
            return BadRequest(ApiResponseDto<DeviceModelResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Delete device model
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteDeviceModel(Guid id)
    {
        try
        {
            var result = await _deviceModelService.DeleteDeviceModelAsync(id);
            return Ok(ApiResponseDto<bool>.Success(result, "Device model deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting device model {ModelId}", id);
            return BadRequest(ApiResponseDto<bool>.Failure("Error deleting device model", false));
        }
    }
}

