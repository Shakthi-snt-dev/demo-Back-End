using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;
using System.Security.Claims;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Customers Controller - Alias for Customer operations (plural route)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(
        ICustomerService customerService,
        ILogger<CustomersController> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    /// <summary>
    /// Get all customers
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<CustomerResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<CustomerResponseDto>>>> GetAll(
        [FromQuery] int? page = null,
        [FromQuery] int? limit = null,
        [FromQuery] string? search = null)
    {
        var result = await _customerService.GetAllCustomersAsync();
        
        // Apply search filter if provided
        if (!string.IsNullOrEmpty(search))
        {
            result = await _customerService.SearchCustomersAsync(search);
        }

        // Apply pagination if provided
        if (page.HasValue && limit.HasValue)
        {
            result = result.Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();
        }
        
        return Ok(ApiResponseDto<IEnumerable<CustomerResponseDto>>.Success(result, "Customers retrieved successfully"));
    }

    /// <summary>
    /// Get customer by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<CustomerResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<CustomerResponseDto>>> GetById(Guid id)
    {
        var result = await _customerService.GetCustomerByIdAsync(id);
        return Ok(ApiResponseDto<CustomerResponseDto>.Success(result, "Customer retrieved successfully"));
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<CustomerResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<CustomerResponseDto>>> Create([FromBody] CreateCustomerRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .Where(e => !string.IsNullOrEmpty(e))
                .ToList();
            
            var errorMessage = errors.Count > 0 
                ? string.Join(", ", errors) 
                : "Invalid request data. Please check all required fields.";
            
            return BadRequest(ApiResponseDto<CustomerResponseDto>.Failure(errorMessage, null));
        }

        // Extract storeId from JWT token claims
        var storeIdClaim = User.FindFirst("store_id")?.Value;
        if (string.IsNullOrEmpty(storeIdClaim) || !Guid.TryParse(storeIdClaim, out var storeId))
        {
            return BadRequest(ApiResponseDto<CustomerResponseDto>.Failure("StoreId is required. Please ensure you are authenticated and have a valid store.", null));
        }

        // Override request.StoreId with the one from token
        request.StoreId = storeId;

        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(ApiResponseDto<CustomerResponseDto>.Failure("Name is required", null));
        }

        var result = await _customerService.CreateCustomerAsync(request);
        return Ok(ApiResponseDto<CustomerResponseDto>.Success(result, "Customer created successfully"));
    }

    /// <summary>
    /// Update customer
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<CustomerResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<CustomerResponseDto>>> Update(Guid id, [FromBody] UpdateCustomerRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<CustomerResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _customerService.UpdateCustomerAsync(id, request);
        return Ok(ApiResponseDto<CustomerResponseDto>.Success(result, "Customer updated successfully"));
    }

    /// <summary>
    /// Delete customer
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> Delete(Guid id)
    {
        var deleted = await _customerService.DeleteCustomerAsync(id);
        if (!deleted)
        {
            return Ok(ApiResponseDto<object>.Failure("Customer not found", null));
        }

        return Ok(ApiResponseDto<object?>.Success(null, "Customer deleted successfully"));
    }

    /// <summary>
    /// Search customers
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<CustomerResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<CustomerResponseDto>>>> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return Ok(ApiResponseDto<IEnumerable<CustomerResponseDto>>.Failure("Search query is required", null));
        }

        var result = await _customerService.SearchCustomersAsync(q);
        return Ok(ApiResponseDto<IEnumerable<CustomerResponseDto>>.Success(result, "Search completed successfully"));
    }
}

