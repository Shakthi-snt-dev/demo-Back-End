using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Presentation.Attributes;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Inventory Items Controller - Manage inventory items
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AppUserOrOwner]
public class InventoryItemsController : ControllerBase
{
    private readonly IInventoryItemService _inventoryItemService;
    private readonly ILogger<InventoryItemsController> _logger;

    public InventoryItemsController(
        IInventoryItemService inventoryItemService,
        ILogger<InventoryItemsController> logger)
    {
        _inventoryItemService = inventoryItemService;
        _logger = logger;
    }

    /// <summary>
    /// Get inventory item by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<InventoryItemResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<InventoryItemResponseDto>>> GetInventoryItemById(Guid id)
    {
        try
        {
            var item = await _inventoryItemService.GetInventoryItemByIdAsync(id);
            return Ok(ApiResponseDto<InventoryItemResponseDto>.Success(item, "Inventory item retrieved successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<InventoryItemResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving inventory item {InventoryItemId}", id);
            return BadRequest(ApiResponseDto<InventoryItemResponseDto>.Failure("Error retrieving inventory item", null));
        }
    }

    /// <summary>
    /// Get inventory items by store ID
    /// </summary>
    [HttpGet("store/{storeId}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<InventoryItemResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<InventoryItemResponseDto>>>> GetInventoryItemsByStore(Guid storeId)
    {
        try
        {
            var items = await _inventoryItemService.GetInventoryItemsByStoreIdAsync(storeId);
            return Ok(ApiResponseDto<IEnumerable<InventoryItemResponseDto>>.Success(items, "Inventory items retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving inventory items for store {StoreId}", storeId);
            return BadRequest(ApiResponseDto<IEnumerable<InventoryItemResponseDto>>.Failure("Error retrieving inventory items", null));
        }
    }

    /// <summary>
    /// Get low stock items for a store
    /// </summary>
    [HttpGet("store/{storeId}/low-stock")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<InventoryItemResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<InventoryItemResponseDto>>>> GetLowStockItems(Guid storeId)
    {
        try
        {
            var items = await _inventoryItemService.GetLowStockItemsAsync(storeId);
            return Ok(ApiResponseDto<IEnumerable<InventoryItemResponseDto>>.Success(items, "Low stock items retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving low stock items for store {StoreId}", storeId);
            return BadRequest(ApiResponseDto<IEnumerable<InventoryItemResponseDto>>.Failure("Error retrieving low stock items", null));
        }
    }

    /// <summary>
    /// Create a new inventory item
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<InventoryItemResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponseDto<InventoryItemResponseDto>>> CreateInventoryItem([FromBody] CreateInventoryItemRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<InventoryItemResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var item = await _inventoryItemService.CreateInventoryItemAsync(request);
            return Ok(ApiResponseDto<InventoryItemResponseDto>.Success(item, "Inventory item created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating inventory item");
            return BadRequest(ApiResponseDto<InventoryItemResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Update inventory item
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<InventoryItemResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<InventoryItemResponseDto>>> UpdateInventoryItem(Guid id, [FromBody] UpdateInventoryItemRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<InventoryItemResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var item = await _inventoryItemService.UpdateInventoryItemAsync(id, request);
            return Ok(ApiResponseDto<InventoryItemResponseDto>.Success(item, "Inventory item updated successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<InventoryItemResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating inventory item {InventoryItemId}", id);
            return BadRequest(ApiResponseDto<InventoryItemResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Add quantity to inventory item
    /// </summary>
    [HttpPost("{id}/add-quantity")]
    [ProducesResponseType(typeof(ApiResponseDto<InventoryItemResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<InventoryItemResponseDto>>> AddQuantity(Guid id, [FromBody] int quantity)
    {
        try
        {
            var item = await _inventoryItemService.AddQuantityAsync(id, quantity);
            return Ok(ApiResponseDto<InventoryItemResponseDto>.Success(item, "Quantity added successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<InventoryItemResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding quantity to inventory item {InventoryItemId}", id);
            return BadRequest(ApiResponseDto<InventoryItemResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Remove quantity from inventory item
    /// </summary>
    [HttpPost("{id}/remove-quantity")]
    [ProducesResponseType(typeof(ApiResponseDto<InventoryItemResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<InventoryItemResponseDto>>> RemoveQuantity(Guid id, [FromBody] int quantity)
    {
        try
        {
            var item = await _inventoryItemService.RemoveQuantityAsync(id, quantity);
            return Ok(ApiResponseDto<InventoryItemResponseDto>.Success(item, "Quantity removed successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<InventoryItemResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing quantity from inventory item {InventoryItemId}", id);
            return BadRequest(ApiResponseDto<InventoryItemResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Reserve quantity for inventory item
    /// </summary>
    [HttpPost("{id}/reserve-quantity")]
    [ProducesResponseType(typeof(ApiResponseDto<InventoryItemResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<InventoryItemResponseDto>>> ReserveQuantity(Guid id, [FromBody] int quantity)
    {
        try
        {
            var item = await _inventoryItemService.ReserveQuantityAsync(id, quantity);
            return Ok(ApiResponseDto<InventoryItemResponseDto>.Success(item, "Quantity reserved successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<InventoryItemResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reserving quantity for inventory item {InventoryItemId}", id);
            return BadRequest(ApiResponseDto<InventoryItemResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Release reserved quantity from inventory item
    /// </summary>
    [HttpPost("{id}/release-reserved-quantity")]
    [ProducesResponseType(typeof(ApiResponseDto<InventoryItemResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<InventoryItemResponseDto>>> ReleaseReservedQuantity(Guid id, [FromBody] int quantity)
    {
        try
        {
            var item = await _inventoryItemService.ReleaseReservedQuantityAsync(id, quantity);
            return Ok(ApiResponseDto<InventoryItemResponseDto>.Success(item, "Reserved quantity released successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<InventoryItemResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error releasing reserved quantity for inventory item {InventoryItemId}", id);
            return BadRequest(ApiResponseDto<InventoryItemResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Delete inventory item
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteInventoryItem(Guid id)
    {
        try
        {
            var result = await _inventoryItemService.DeleteInventoryItemAsync(id);
            return Ok(ApiResponseDto<bool>.Success(result, "Inventory item deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting inventory item {InventoryItemId}", id);
            return BadRequest(ApiResponseDto<bool>.Failure("Error deleting inventory item", false));
        }
    }
}

