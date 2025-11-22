using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Presentation.Attributes;
using Flowtap_Application.Services;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Customers Controller - Manage customers by store
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AppUserOrOwner]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IHttpAccessorService _httpAccessorService;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(
        ICustomerService customerService,
        IHttpAccessorService httpAccessorService,
        ILogger<CustomersController> logger)
    {
        _customerService = customerService;
        _httpAccessorService = httpAccessorService;
        _logger = logger;
    }

    /// <summary>
    /// Get customer by ID
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <returns>Customer details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<CustomerResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<CustomerResponseDto>>> GetCustomerById(Guid id)
    {
        try
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            return Ok(ApiResponseDto<CustomerResponseDto>.Success(customer, "Customer retrieved successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<CustomerResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer {CustomerId}", id);
            return BadRequest(ApiResponseDto<CustomerResponseDto>.Failure("Error retrieving customer", null));
        }
    }

    /// <summary>
    /// Get all customers for the authenticated AppUser's stores
    /// </summary>
    /// <returns>List of customers belonging to the authenticated AppUser's stores</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<CustomerResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<CustomerResponseDto>>>> GetAllCustomers()
    {
        try
        {
            // Get UserAccount ID from JWT token
            var userAccountId = _httpAccessorService.GetUserAccountId();
            if (!userAccountId.HasValue)
            {
                return Unauthorized(ApiResponseDto<IEnumerable<CustomerResponseDto>>.Failure("User is not authenticated or UserAccount ID is missing", null));
            }

            var customers = await _customerService.GetCustomersByUserAccountIdAsync(userAccountId.Value);
            return Ok(ApiResponseDto<IEnumerable<CustomerResponseDto>>.Success(customers, $"Retrieved {customers.Count()} customers"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponseDto<IEnumerable<CustomerResponseDto>>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customers");
            return BadRequest(ApiResponseDto<IEnumerable<CustomerResponseDto>>.Failure("Error retrieving customers", null));
        }
    }

    /// <summary>
    /// Get customers by store ID
    /// </summary>
    /// <param name="storeId">Store ID</param>
    /// <returns>List of customers for the store</returns>
    [HttpGet("store/{storeId}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<CustomerResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<CustomerResponseDto>>>> GetCustomersByStore(Guid storeId)
    {
        try
        {
            var customers = await _customerService.GetCustomersByStoreIdAsync(storeId);
            return Ok(ApiResponseDto<IEnumerable<CustomerResponseDto>>.Success(customers, $"Retrieved {customers.Count()} customers for store"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<IEnumerable<CustomerResponseDto>>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customers for store {StoreId}", storeId);
            return BadRequest(ApiResponseDto<IEnumerable<CustomerResponseDto>>.Failure("Error retrieving customers", null));
        }
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    /// <param name="request">Customer creation request (must include StoreId)</param>
    /// <returns>Created customer</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<CustomerResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<CustomerResponseDto>>> CreateCustomer(
        [FromBody] CreateCustomerRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<CustomerResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var customer = await _customerService.CreateCustomerAsync(request);
            return Ok(ApiResponseDto<CustomerResponseDto>.Success(customer, "Customer created successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<CustomerResponseDto>.Failure(ex.Message, null));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponseDto<CustomerResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer for store {StoreId}", request.StoreId);
            return BadRequest(ApiResponseDto<CustomerResponseDto>.Failure("Error creating customer", null));
        }
    }

    /// <summary>
    /// Update customer
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <param name="request">Customer update request</param>
    /// <returns>Updated customer</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<CustomerResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<CustomerResponseDto>>> UpdateCustomer(
        Guid id,
        [FromBody] UpdateCustomerRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<CustomerResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var customer = await _customerService.UpdateCustomerAsync(id, request);
            return Ok(ApiResponseDto<CustomerResponseDto>.Success(customer, "Customer updated successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<CustomerResponseDto>.Failure(ex.Message, null));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponseDto<CustomerResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating customer {CustomerId}", id);
            return BadRequest(ApiResponseDto<CustomerResponseDto>.Failure("Error updating customer", null));
        }
    }
}

