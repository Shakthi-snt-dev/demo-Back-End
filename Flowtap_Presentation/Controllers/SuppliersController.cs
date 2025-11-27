using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Presentation.Attributes;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Suppliers Controller - Manage suppliers
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AppUserOrOwner]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;
    private readonly ILogger<SuppliersController> _logger;

    public SuppliersController(
        ISupplierService supplierService,
        ILogger<SuppliersController> logger)
    {
        _supplierService = supplierService;
        _logger = logger;
    }

    /// <summary>
    /// Get supplier by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<SupplierResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<SupplierResponseDto>>> GetSupplierById(Guid id)
    {
        try
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            return Ok(ApiResponseDto<SupplierResponseDto>.Success(supplier, "Supplier retrieved successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<SupplierResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving supplier {SupplierId}", id);
            return BadRequest(ApiResponseDto<SupplierResponseDto>.Failure("Error retrieving supplier", null));
        }
    }

    /// <summary>
    /// Get all suppliers
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<SupplierResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<SupplierResponseDto>>>> GetAllSuppliers()
    {
        try
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(ApiResponseDto<IEnumerable<SupplierResponseDto>>.Success(suppliers, "Suppliers retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving suppliers");
            return BadRequest(ApiResponseDto<IEnumerable<SupplierResponseDto>>.Failure("Error retrieving suppliers", null));
        }
    }

    /// <summary>
    /// Get active suppliers
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<SupplierResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<SupplierResponseDto>>>> GetActiveSuppliers()
    {
        try
        {
            var suppliers = await _supplierService.GetActiveSuppliersAsync();
            return Ok(ApiResponseDto<IEnumerable<SupplierResponseDto>>.Success(suppliers, "Active suppliers retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active suppliers");
            return BadRequest(ApiResponseDto<IEnumerable<SupplierResponseDto>>.Failure("Error retrieving active suppliers", null));
        }
    }

    /// <summary>
    /// Create a new supplier
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<SupplierResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponseDto<SupplierResponseDto>>> CreateSupplier([FromBody] CreateSupplierRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<SupplierResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var supplier = await _supplierService.CreateSupplierAsync(request);
            return Ok(ApiResponseDto<SupplierResponseDto>.Success(supplier, "Supplier created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating supplier");
            return BadRequest(ApiResponseDto<SupplierResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Update supplier
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<SupplierResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<SupplierResponseDto>>> UpdateSupplier(Guid id, [FromBody] UpdateSupplierRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<SupplierResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var supplier = await _supplierService.UpdateSupplierAsync(id, request);
            return Ok(ApiResponseDto<SupplierResponseDto>.Success(supplier, "Supplier updated successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<SupplierResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating supplier {SupplierId}", id);
            return BadRequest(ApiResponseDto<SupplierResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Delete supplier
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteSupplier(Guid id)
    {
        try
        {
            var result = await _supplierService.DeleteSupplierAsync(id);
            return Ok(ApiResponseDto<bool>.Success(result, "Supplier deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting supplier {SupplierId}", id);
            return BadRequest(ApiResponseDto<bool>.Failure("Error deleting supplier", false));
        }
    }
}

