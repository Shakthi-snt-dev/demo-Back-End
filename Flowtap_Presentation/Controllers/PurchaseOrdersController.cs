using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Presentation.Attributes;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Purchase Orders Controller - Manage purchase orders
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AppUserOrOwner]
public class PurchaseOrdersController : ControllerBase
{
    private readonly IPurchaseOrderService _purchaseOrderService;
    private readonly ILogger<PurchaseOrdersController> _logger;

    public PurchaseOrdersController(
        IPurchaseOrderService purchaseOrderService,
        ILogger<PurchaseOrdersController> logger)
    {
        _purchaseOrderService = purchaseOrderService;
        _logger = logger;
    }

    /// <summary>
    /// Get purchase order by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<PurchaseOrderResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<PurchaseOrderResponseDto>>> GetPurchaseOrderById(Guid id)
    {
        try
        {
            var purchaseOrder = await _purchaseOrderService.GetPurchaseOrderByIdAsync(id);
            return Ok(ApiResponseDto<PurchaseOrderResponseDto>.Success(purchaseOrder, "Purchase order retrieved successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<PurchaseOrderResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving purchase order {PurchaseOrderId}", id);
            return BadRequest(ApiResponseDto<PurchaseOrderResponseDto>.Failure("Error retrieving purchase order", null));
        }
    }

    /// <summary>
    /// Get purchase orders by store ID
    /// </summary>
    [HttpGet("store/{storeId}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<PurchaseOrderResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<PurchaseOrderResponseDto>>>> GetPurchaseOrdersByStore(Guid storeId)
    {
        try
        {
            var purchaseOrders = await _purchaseOrderService.GetPurchaseOrdersByStoreIdAsync(storeId);
            return Ok(ApiResponseDto<IEnumerable<PurchaseOrderResponseDto>>.Success(purchaseOrders, "Purchase orders retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving purchase orders for store {StoreId}", storeId);
            return BadRequest(ApiResponseDto<IEnumerable<PurchaseOrderResponseDto>>.Failure("Error retrieving purchase orders", null));
        }
    }

    /// <summary>
    /// Create a new purchase order
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<PurchaseOrderResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponseDto<PurchaseOrderResponseDto>>> CreatePurchaseOrder([FromBody] CreatePurchaseOrderRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<PurchaseOrderResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var purchaseOrder = await _purchaseOrderService.CreatePurchaseOrderAsync(request);
            return Ok(ApiResponseDto<PurchaseOrderResponseDto>.Success(purchaseOrder, "Purchase order created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating purchase order");
            return BadRequest(ApiResponseDto<PurchaseOrderResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Submit purchase order
    /// </summary>
    [HttpPost("{id}/submit")]
    [ProducesResponseType(typeof(ApiResponseDto<PurchaseOrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<PurchaseOrderResponseDto>>> SubmitPurchaseOrder(Guid id)
    {
        try
        {
            var purchaseOrder = await _purchaseOrderService.SubmitPurchaseOrderAsync(id);
            return Ok(ApiResponseDto<PurchaseOrderResponseDto>.Success(purchaseOrder, "Purchase order submitted successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<PurchaseOrderResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting purchase order {PurchaseOrderId}", id);
            return BadRequest(ApiResponseDto<PurchaseOrderResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Mark purchase order as received
    /// </summary>
    [HttpPost("{id}/mark-received")]
    [ProducesResponseType(typeof(ApiResponseDto<PurchaseOrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<PurchaseOrderResponseDto>>> MarkAsReceived(Guid id)
    {
        try
        {
            var purchaseOrder = await _purchaseOrderService.MarkAsReceivedAsync(id);
            return Ok(ApiResponseDto<PurchaseOrderResponseDto>.Success(purchaseOrder, "Purchase order marked as received"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<PurchaseOrderResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking purchase order as received {PurchaseOrderId}", id);
            return BadRequest(ApiResponseDto<PurchaseOrderResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Cancel purchase order
    /// </summary>
    [HttpPost("{id}/cancel")]
    [ProducesResponseType(typeof(ApiResponseDto<PurchaseOrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<PurchaseOrderResponseDto>>> CancelPurchaseOrder(Guid id)
    {
        try
        {
            var purchaseOrder = await _purchaseOrderService.CancelPurchaseOrderAsync(id);
            return Ok(ApiResponseDto<PurchaseOrderResponseDto>.Success(purchaseOrder, "Purchase order cancelled successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<PurchaseOrderResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling purchase order {PurchaseOrderId}", id);
            return BadRequest(ApiResponseDto<PurchaseOrderResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Delete purchase order
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeletePurchaseOrder(Guid id)
    {
        try
        {
            var result = await _purchaseOrderService.DeletePurchaseOrderAsync(id);
            return Ok(ApiResponseDto<bool>.Success(result, "Purchase order deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting purchase order {PurchaseOrderId}", id);
            return BadRequest(ApiResponseDto<bool>.Failure("Error deleting purchase order", false));
        }
    }
}

