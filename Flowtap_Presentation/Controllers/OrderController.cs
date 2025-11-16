using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderController> _logger;

    public OrderController(
        IOrderService orderService,
        ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new order
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<OrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<OrderResponseDto>>> CreateOrder([FromBody] CreateOrderRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<OrderResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _orderService.CreateOrderAsync(request);
        return Ok(ApiResponseDto<OrderResponseDto>.Success(result, "Order created successfully"));
    }

    /// <summary>
    /// Get order by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<OrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<OrderResponseDto>>> GetOrder(Guid id)
    {
        var result = await _orderService.GetOrderByIdAsync(id);
        return Ok(ApiResponseDto<OrderResponseDto>.Success(result, "Order retrieved successfully"));
    }

    /// <summary>
    /// Get order by order number
    /// </summary>
    [HttpGet("number/{orderNumber}")]
    [ProducesResponseType(typeof(ApiResponseDto<OrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<OrderResponseDto>>> GetOrderByNumber(string orderNumber)
    {
        var result = await _orderService.GetOrderByNumberAsync(orderNumber);
        if (result == null)
        {
            return Ok(ApiResponseDto<OrderResponseDto>.Failure("Order not found", null));
        }

        return Ok(ApiResponseDto<OrderResponseDto>.Success(result, "Order retrieved successfully"));
    }

    /// <summary>
    /// Get all orders
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<OrderResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<OrderResponseDto>>>> GetAllOrders()
    {
        var result = await _orderService.GetAllOrdersAsync();
        return Ok(ApiResponseDto<IEnumerable<OrderResponseDto>>.Success(result, "Orders retrieved successfully"));
    }

    /// <summary>
    /// Get orders by customer ID
    /// </summary>
    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<OrderResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<OrderResponseDto>>>> GetOrdersByCustomer(Guid customerId)
    {
        var result = await _orderService.GetOrdersByCustomerIdAsync(customerId);
        return Ok(ApiResponseDto<IEnumerable<OrderResponseDto>>.Success(result, "Orders retrieved successfully"));
    }

    /// <summary>
    /// Get orders by store ID
    /// </summary>
    [HttpGet("store/{storeId}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<OrderResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<OrderResponseDto>>>> GetOrdersByStore(Guid storeId)
    {
        var result = await _orderService.GetOrdersByStoreIdAsync(storeId);
        return Ok(ApiResponseDto<IEnumerable<OrderResponseDto>>.Success(result, "Orders retrieved successfully"));
    }

    /// <summary>
    /// Get orders by status
    /// </summary>
    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<OrderResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<OrderResponseDto>>>> GetOrdersByStatus(string status)
    {
        var result = await _orderService.GetOrdersByStatusAsync(status);
        return Ok(ApiResponseDto<IEnumerable<OrderResponseDto>>.Success(result, "Orders retrieved successfully"));
    }

    /// <summary>
    /// Get orders by date range
    /// </summary>
    [HttpGet("date-range")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<OrderResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<OrderResponseDto>>>> GetOrdersByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var result = await _orderService.GetOrdersByDateRangeAsync(startDate, endDate);
        return Ok(ApiResponseDto<IEnumerable<OrderResponseDto>>.Success(result, "Orders retrieved successfully"));
    }

    /// <summary>
    /// Update order
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<OrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<OrderResponseDto>>> UpdateOrder(Guid id, [FromBody] UpdateOrderRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<OrderResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _orderService.UpdateOrderAsync(id, request);
        return Ok(ApiResponseDto<OrderResponseDto>.Success(result, "Order updated successfully"));
    }

    /// <summary>
    /// Complete order
    /// </summary>
    [HttpPost("{id}/complete")]
    [ProducesResponseType(typeof(ApiResponseDto<OrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<OrderResponseDto>>> CompleteOrder(Guid id)
    {
        var result = await _orderService.CompleteOrderAsync(id);
        return Ok(ApiResponseDto<OrderResponseDto>.Success(result, "Order completed successfully"));
    }

    /// <summary>
    /// Cancel order
    /// </summary>
    [HttpPost("{id}/cancel")]
    [ProducesResponseType(typeof(ApiResponseDto<OrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<OrderResponseDto>>> CancelOrder(Guid id)
    {
        var result = await _orderService.CancelOrderAsync(id);
        return Ok(ApiResponseDto<OrderResponseDto>.Success(result, "Order cancelled successfully"));
    }

    /// <summary>
    /// Delete order
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> DeleteOrder(Guid id)
    {
        var deleted = await _orderService.DeleteOrderAsync(id);
        if (!deleted)
        {
            return Ok(ApiResponseDto<object>.Failure("Order not found", null));
        }

        return Ok(ApiResponseDto<object?>.Success(null, "Order deleted successfully"));
    }
}

