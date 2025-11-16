using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Inventory Controller - Alias for Product operations
/// </summary>
[ApiController]
[Route("api/inventory")]
public class InventoryController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<InventoryController> _logger;

    public InventoryController(
        IProductService productService,
        ILogger<InventoryController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Get all inventory items (products)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ProductResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductResponseDto>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10,
        [FromQuery] string? category = null)
    {
        var result = await _productService.GetAllProductsAsync();
        
        // Apply category filter if provided
        if (!string.IsNullOrEmpty(category))
        {
            result = result.Where(p => p.Category?.Equals(category, StringComparison.OrdinalIgnoreCase) == true).ToList();
        }

        // Apply pagination
        var paginatedResult = result.Skip((page - 1) * limit).Take(limit).ToList();
        
        return Ok(ApiResponseDto<IEnumerable<ProductResponseDto>>.Success(paginatedResult, "Inventory items retrieved successfully"));
    }

    /// <summary>
    /// Get inventory item by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<ProductResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<ProductResponseDto>>> GetById(Guid id)
    {
        var result = await _productService.GetProductByIdAsync(id);
        return Ok(ApiResponseDto<ProductResponseDto>.Success(result, "Inventory item retrieved successfully"));
    }

    /// <summary>
    /// Create a new inventory item (product)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<ProductResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<ProductResponseDto>>> Create([FromBody] CreateProductRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<ProductResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _productService.CreateProductAsync(request);
        return Ok(ApiResponseDto<ProductResponseDto>.Success(result, "Inventory item created successfully"));
    }

    /// <summary>
    /// Update inventory item
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<ProductResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<ProductResponseDto>>> Update(Guid id, [FromBody] UpdateProductRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<ProductResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _productService.UpdateProductAsync(id, request);
        return Ok(ApiResponseDto<ProductResponseDto>.Success(result, "Inventory item updated successfully"));
    }

    /// <summary>
    /// Delete inventory item
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> Delete(Guid id)
    {
        var deleted = await _productService.DeleteProductAsync(id);
        if (!deleted)
        {
            return Ok(ApiResponseDto<object>.Failure("Inventory item not found", null));
        }

        return Ok(ApiResponseDto<object?>.Success(null, "Inventory item deleted successfully"));
    }

    /// <summary>
    /// Update stock quantity
    /// </summary>
    [HttpPatch("{id}/stock")]
    [ProducesResponseType(typeof(ApiResponseDto<ProductResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<ProductResponseDto>>> UpdateStock(
        Guid id,
        [FromBody] UpdateStockRequestDto request)
    {
        var result = await _productService.UpdateStockAsync(id, request.Quantity);
        return Ok(ApiResponseDto<ProductResponseDto>.Success(result, "Stock updated successfully"));
    }
}

// DTO for stock update
public class UpdateStockRequestDto
{
    public int Quantity { get; set; }
}

