using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// POS (Point of Sale) Controller - Alias for Order operations
/// </summary>
[ApiController]
[Route("api/pos")]
public class POSController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<POSController> _logger;

    public POSController(
        IOrderService orderService,
        ILogger<POSController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new sale/order
    /// </summary>
    [HttpPost("sales")]
    [ProducesResponseType(typeof(ApiResponseDto<OrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<OrderResponseDto>>> CreateSale([FromBody] CreateOrderRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<OrderResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _orderService.CreateOrderAsync(request);
        return Ok(ApiResponseDto<OrderResponseDto>.Success(result, "Sale created successfully"));
    }

    /// <summary>
    /// Get all sales/orders
    /// </summary>
    [HttpGet("sales")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<OrderResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<OrderResponseDto>>>> GetSales(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10,
        [FromQuery] string? date = null)
    {
        var result = await _orderService.GetAllOrdersAsync();
        
        // Apply date filter if provided
        if (!string.IsNullOrEmpty(date) && DateTime.TryParse(date, out var filterDate))
        {
            result = result.Where(o => o.CreatedAt.Date == filterDate.Date).ToList();
        }

        // Apply pagination
        var paginatedResult = result.Skip((page - 1) * limit).Take(limit).ToList();
        
        return Ok(ApiResponseDto<IEnumerable<OrderResponseDto>>.Success(paginatedResult, "Sales retrieved successfully"));
    }

    /// <summary>
    /// Get sale by ID
    /// </summary>
    [HttpGet("sales/{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<OrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<OrderResponseDto>>> GetSaleById(Guid id)
    {
        var result = await _orderService.GetOrderByIdAsync(id);
        return Ok(ApiResponseDto<OrderResponseDto>.Success(result, "Sale retrieved successfully"));
    }

    /// <summary>
    /// Process payment for a sale
    /// </summary>
    [HttpPost("sales/{id}/payment")]
    [ProducesResponseType(typeof(ApiResponseDto<OrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<OrderResponseDto>>> ProcessPayment(
        Guid id,
        [FromBody] ProcessPaymentRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<OrderResponseDto>.Failure("Invalid request data", null));
        }

        // Update order with payment information and complete it
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
        {
            return Ok(ApiResponseDto<OrderResponseDto>.Failure("Sale not found", null));
        }

        // Complete the order after payment
        var result = await _orderService.CompleteOrderAsync(id);
        return Ok(ApiResponseDto<OrderResponseDto>.Success(result, "Payment processed successfully"));
    }

    /// <summary>
    /// Get receipt for a sale
    /// </summary>
    [HttpGet("sales/{id}/receipt")]
    [ProducesResponseType(typeof(ApiResponseDto<OrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<OrderResponseDto>>> GetReceipt(Guid id)
    {
        var result = await _orderService.GetOrderByIdAsync(id);
        return Ok(ApiResponseDto<OrderResponseDto>.Success(result, "Receipt retrieved successfully"));
    }
}

// DTO for payment processing
public class ProcessPaymentRequestDto
{
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? TransactionId { get; set; }
}

