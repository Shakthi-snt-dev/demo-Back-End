using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Presentation.Attributes;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Services Controller - Manage repair services
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AppUserOrOwner]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _serviceService;
    private readonly ILogger<ServicesController> _logger;

    public ServicesController(
        IServiceService serviceService,
        ILogger<ServicesController> logger)
    {
        _serviceService = serviceService;
        _logger = logger;
    }

    /// <summary>
    /// Get service by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<ServiceResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<ServiceResponseDto>>> GetServiceById(Guid id)
    {
        try
        {
            var service = await _serviceService.GetServiceByIdAsync(id);
            return Ok(ApiResponseDto<ServiceResponseDto>.Success(service, "Service retrieved successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<ServiceResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving service {ServiceId}", id);
            return BadRequest(ApiResponseDto<ServiceResponseDto>.Failure("Error retrieving service", null));
        }
    }

    /// <summary>
    /// Get services by store ID
    /// </summary>
    [HttpGet("store/{storeId}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ServiceResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<ServiceResponseDto>>>> GetServicesByStore(Guid storeId)
    {
        try
        {
            var services = await _serviceService.GetServicesByStoreIdAsync(storeId);
            return Ok(ApiResponseDto<IEnumerable<ServiceResponseDto>>.Success(services, "Services retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving services for store {StoreId}", storeId);
            return BadRequest(ApiResponseDto<IEnumerable<ServiceResponseDto>>.Failure("Error retrieving services", null));
        }
    }

    /// <summary>
    /// Get active services by store ID
    /// </summary>
    [HttpGet("store/{storeId}/active")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ServiceResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<ServiceResponseDto>>>> GetActiveServices(Guid storeId)
    {
        try
        {
            var services = await _serviceService.GetActiveServicesAsync(storeId);
            return Ok(ApiResponseDto<IEnumerable<ServiceResponseDto>>.Success(services, "Active services retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active services for store {StoreId}", storeId);
            return BadRequest(ApiResponseDto<IEnumerable<ServiceResponseDto>>.Failure("Error retrieving active services", null));
        }
    }

    /// <summary>
    /// Create a new service
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<ServiceResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponseDto<ServiceResponseDto>>> CreateService([FromBody] CreateServiceRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<ServiceResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var service = await _serviceService.CreateServiceAsync(request);
            return Ok(ApiResponseDto<ServiceResponseDto>.Success(service, "Service created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating service");
            return BadRequest(ApiResponseDto<ServiceResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Update service
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<ServiceResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<ServiceResponseDto>>> UpdateService(Guid id, [FromBody] UpdateServiceRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<ServiceResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var service = await _serviceService.UpdateServiceAsync(id, request);
            return Ok(ApiResponseDto<ServiceResponseDto>.Success(service, "Service updated successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<ServiceResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating service {ServiceId}", id);
            return BadRequest(ApiResponseDto<ServiceResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Delete service
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteService(Guid id)
    {
        try
        {
            var result = await _serviceService.DeleteServiceAsync(id);
            return Ok(ApiResponseDto<bool>.Success(result, "Service deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting service {ServiceId}", id);
            return BadRequest(ApiResponseDto<bool>.Failure("Error deleting service", false));
        }
    }
}

