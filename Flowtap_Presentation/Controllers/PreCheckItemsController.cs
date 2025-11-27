using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Presentation.Attributes;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Pre-Check Items Controller - Manage pre-repair checklist items
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AppUserOrOwner]
public class PreCheckItemsController : ControllerBase
{
    private readonly IPreCheckItemService _preCheckItemService;
    private readonly ILogger<PreCheckItemsController> _logger;

    public PreCheckItemsController(
        IPreCheckItemService preCheckItemService,
        ILogger<PreCheckItemsController> logger)
    {
        _preCheckItemService = preCheckItemService;
        _logger = logger;
    }

    /// <summary>
    /// Get pre-check item by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<PreCheckItemResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<PreCheckItemResponseDto>>> GetPreCheckItemById(Guid id)
    {
        try
        {
            var item = await _preCheckItemService.GetPreCheckItemByIdAsync(id);
            return Ok(ApiResponseDto<PreCheckItemResponseDto>.Success(item, "Pre-check item retrieved successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<PreCheckItemResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving pre-check item {ItemId}", id);
            return BadRequest(ApiResponseDto<PreCheckItemResponseDto>.Failure("Error retrieving pre-check item", null));
        }
    }

    /// <summary>
    /// Get pre-check items by store ID
    /// </summary>
    [HttpGet("store/{storeId}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<PreCheckItemResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<PreCheckItemResponseDto>>>> GetPreCheckItemsByStore(Guid storeId)
    {
        try
        {
            var items = await _preCheckItemService.GetPreCheckItemsByStoreIdAsync(storeId);
            return Ok(ApiResponseDto<IEnumerable<PreCheckItemResponseDto>>.Success(items, "Pre-check items retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving pre-check items for store {StoreId}", storeId);
            return BadRequest(ApiResponseDto<IEnumerable<PreCheckItemResponseDto>>.Failure("Error retrieving pre-check items", null));
        }
    }

    /// <summary>
    /// Get active pre-check items by store ID
    /// </summary>
    [HttpGet("store/{storeId}/active")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<PreCheckItemResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<PreCheckItemResponseDto>>>> GetActivePreCheckItems(Guid storeId)
    {
        try
        {
            var items = await _preCheckItemService.GetActivePreCheckItemsAsync(storeId);
            return Ok(ApiResponseDto<IEnumerable<PreCheckItemResponseDto>>.Success(items, "Active pre-check items retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active pre-check items for store {StoreId}", storeId);
            return BadRequest(ApiResponseDto<IEnumerable<PreCheckItemResponseDto>>.Failure("Error retrieving active pre-check items", null));
        }
    }

    /// <summary>
    /// Create a new pre-check item
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<PreCheckItemResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponseDto<PreCheckItemResponseDto>>> CreatePreCheckItem([FromBody] CreatePreCheckItemRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<PreCheckItemResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var item = await _preCheckItemService.CreatePreCheckItemAsync(request);
            return Ok(ApiResponseDto<PreCheckItemResponseDto>.Success(item, "Pre-check item created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating pre-check item");
            return BadRequest(ApiResponseDto<PreCheckItemResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Update pre-check item
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<PreCheckItemResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<PreCheckItemResponseDto>>> UpdatePreCheckItem(Guid id, [FromBody] UpdatePreCheckItemRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<PreCheckItemResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var item = await _preCheckItemService.UpdatePreCheckItemAsync(id, request);
            return Ok(ApiResponseDto<PreCheckItemResponseDto>.Success(item, "Pre-check item updated successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<PreCheckItemResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating pre-check item {ItemId}", id);
            return BadRequest(ApiResponseDto<PreCheckItemResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Delete pre-check item
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeletePreCheckItem(Guid id)
    {
        try
        {
            var result = await _preCheckItemService.DeletePreCheckItemAsync(id);
            return Ok(ApiResponseDto<bool>.Success(result, "Pre-check item deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting pre-check item {ItemId}", id);
            return BadRequest(ApiResponseDto<bool>.Failure("Error deleting pre-check item", false));
        }
    }
}

