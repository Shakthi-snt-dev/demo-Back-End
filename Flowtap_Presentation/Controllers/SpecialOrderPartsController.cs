using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Presentation.Attributes;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Special Order Parts Controller - Manage special order parts
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AppUserOrOwner]
public class SpecialOrderPartsController : ControllerBase
{
    private readonly ISpecialOrderPartService _specialOrderPartService;
    private readonly ILogger<SpecialOrderPartsController> _logger;

    public SpecialOrderPartsController(
        ISpecialOrderPartService specialOrderPartService,
        ILogger<SpecialOrderPartsController> logger)
    {
        _specialOrderPartService = specialOrderPartService;
        _logger = logger;
    }

    /// <summary>
    /// Get special order part by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<SpecialOrderPartResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<SpecialOrderPartResponseDto>>> GetSpecialOrderPartById(Guid id)
    {
        try
        {
            var part = await _specialOrderPartService.GetSpecialOrderPartByIdAsync(id);
            return Ok(ApiResponseDto<SpecialOrderPartResponseDto>.Success(part, "Special order part retrieved successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<SpecialOrderPartResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving special order part {PartId}", id);
            return BadRequest(ApiResponseDto<SpecialOrderPartResponseDto>.Failure("Error retrieving special order part", null));
        }
    }

    /// <summary>
    /// Get special order parts by store ID
    /// </summary>
    [HttpGet("store/{storeId}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<SpecialOrderPartResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<SpecialOrderPartResponseDto>>>> GetSpecialOrderPartsByStore(Guid storeId)
    {
        try
        {
            var parts = await _specialOrderPartService.GetSpecialOrderPartsByStoreIdAsync(storeId);
            return Ok(ApiResponseDto<IEnumerable<SpecialOrderPartResponseDto>>.Success(parts, "Special order parts retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving special order parts for store {StoreId}", storeId);
            return BadRequest(ApiResponseDto<IEnumerable<SpecialOrderPartResponseDto>>.Failure("Error retrieving special order parts", null));
        }
    }

    /// <summary>
    /// Get pending special order parts by store ID
    /// </summary>
    [HttpGet("store/{storeId}/pending")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<SpecialOrderPartResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<SpecialOrderPartResponseDto>>>> GetPendingOrders(Guid storeId)
    {
        try
        {
            var parts = await _specialOrderPartService.GetPendingOrdersAsync(storeId);
            return Ok(ApiResponseDto<IEnumerable<SpecialOrderPartResponseDto>>.Success(parts, "Pending orders retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving pending orders for store {StoreId}", storeId);
            return BadRequest(ApiResponseDto<IEnumerable<SpecialOrderPartResponseDto>>.Failure("Error retrieving pending orders", null));
        }
    }

    /// <summary>
    /// Create a new special order part
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<SpecialOrderPartResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponseDto<SpecialOrderPartResponseDto>>> CreateSpecialOrderPart([FromBody] CreateSpecialOrderPartRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<SpecialOrderPartResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var part = await _specialOrderPartService.CreateSpecialOrderPartAsync(request);
            return Ok(ApiResponseDto<SpecialOrderPartResponseDto>.Success(part, "Special order part created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating special order part");
            return BadRequest(ApiResponseDto<SpecialOrderPartResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Update special order part
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<SpecialOrderPartResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<SpecialOrderPartResponseDto>>> UpdateSpecialOrderPart(Guid id, [FromBody] UpdateSpecialOrderPartRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<SpecialOrderPartResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var part = await _specialOrderPartService.UpdateSpecialOrderPartAsync(id, request);
            return Ok(ApiResponseDto<SpecialOrderPartResponseDto>.Success(part, "Special order part updated successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<SpecialOrderPartResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating special order part {PartId}", id);
            return BadRequest(ApiResponseDto<SpecialOrderPartResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Mark special order part as received
    /// </summary>
    [HttpPost("{id}/mark-received")]
    [ProducesResponseType(typeof(ApiResponseDto<SpecialOrderPartResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<SpecialOrderPartResponseDto>>> MarkAsReceived(Guid id, [FromBody] DateTime? receivedDate = null)
    {
        try
        {
            var part = await _specialOrderPartService.MarkAsReceivedAsync(id, receivedDate);
            return Ok(ApiResponseDto<SpecialOrderPartResponseDto>.Success(part, "Special order part marked as received"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<SpecialOrderPartResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking special order part {PartId} as received", id);
            return BadRequest(ApiResponseDto<SpecialOrderPartResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Delete special order part
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteSpecialOrderPart(Guid id)
    {
        try
        {
            var result = await _specialOrderPartService.DeleteSpecialOrderPartAsync(id);
            return Ok(ApiResponseDto<bool>.Success(result, "Special order part deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting special order part {PartId}", id);
            return BadRequest(ApiResponseDto<bool>.Failure("Error deleting special order part", false));
        }
    }
}

