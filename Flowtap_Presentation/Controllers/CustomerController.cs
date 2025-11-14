using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(
        ICustomerService customerService,
        ILogger<CustomerController> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<CustomerResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<CustomerResponseDto>>> CreateCustomer([FromBody] CreateCustomerRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<CustomerResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _customerService.CreateCustomerAsync(request);
        return Ok(ApiResponseDto<CustomerResponseDto>.Success(result, "Customer created successfully"));
    }

    /// <summary>
    /// Get customer by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<CustomerResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<CustomerResponseDto>>> GetCustomer(Guid id)
    {
        var result = await _customerService.GetCustomerByIdAsync(id);
        return Ok(ApiResponseDto<CustomerResponseDto>.Success(result, "Customer retrieved successfully"));
    }

    /// <summary>
    /// Get all customers
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<CustomerResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<CustomerResponseDto>>>> GetAllCustomers()
    {
        var result = await _customerService.GetAllCustomersAsync();
        return Ok(ApiResponseDto<IEnumerable<CustomerResponseDto>>.Success(result, "Customers retrieved successfully"));
    }

    /// <summary>
    /// Search customers
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<CustomerResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<CustomerResponseDto>>>> SearchCustomers([FromQuery] string term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return Ok(ApiResponseDto<IEnumerable<CustomerResponseDto>>.Failure("Search term is required", null));
        }

        var result = await _customerService.SearchCustomersAsync(term);
        return Ok(ApiResponseDto<IEnumerable<CustomerResponseDto>>.Success(result, "Search completed successfully"));
    }

    /// <summary>
    /// Get customers by status
    /// </summary>
    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<CustomerResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<CustomerResponseDto>>>> GetCustomersByStatus(string status)
    {
        var result = await _customerService.GetCustomersByStatusAsync(status);
        return Ok(ApiResponseDto<IEnumerable<CustomerResponseDto>>.Success(result, "Customers retrieved successfully"));
    }

    /// <summary>
    /// Update customer
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<CustomerResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<CustomerResponseDto>>> UpdateCustomer(Guid id, [FromBody] UpdateCustomerRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<CustomerResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _customerService.UpdateCustomerAsync(id, request);
        return Ok(ApiResponseDto<CustomerResponseDto>.Success(result, "Customer updated successfully"));
    }

    /// <summary>
    /// Mark customer as VIP
    /// </summary>
    [HttpPost("{id}/vip")]
    [ProducesResponseType(typeof(ApiResponseDto<CustomerResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<CustomerResponseDto>>> MarkAsVip(Guid id)
    {
        var result = await _customerService.MarkCustomerAsVipAsync(id);
        return Ok(ApiResponseDto<CustomerResponseDto>.Success(result, "Customer marked as VIP successfully"));
    }

    /// <summary>
    /// Delete customer
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> DeleteCustomer(Guid id)
    {
        var deleted = await _customerService.DeleteCustomerAsync(id);
        if (!deleted)
        {
            return Ok(ApiResponseDto<object>.Failure("Customer not found", null));
        }

        return Ok(ApiResponseDto<object>.Success(null, "Customer deleted successfully"));
    }
}

