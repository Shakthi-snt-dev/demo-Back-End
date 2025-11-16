using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(
        IProductService productService,
        ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<ProductResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<ProductResponseDto>>> CreateProduct([FromBody] CreateProductRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<ProductResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _productService.CreateProductAsync(request);
        return Ok(ApiResponseDto<ProductResponseDto>.Success(result, "Product created successfully"));
    }

    /// <summary>
    /// Get product by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<ProductResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<ProductResponseDto>>> GetProduct(Guid id)
    {
        var result = await _productService.GetProductByIdAsync(id);
        return Ok(ApiResponseDto<ProductResponseDto>.Success(result, "Product retrieved successfully"));
    }

    /// <summary>
    /// Get product by SKU
    /// </summary>
    [HttpGet("sku/{sku}")]
    [ProducesResponseType(typeof(ApiResponseDto<ProductResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<ProductResponseDto>>> GetProductBySku(string sku)
    {
        var result = await _productService.GetProductBySkuAsync(sku);
        if (result == null)
        {
            return Ok(ApiResponseDto<ProductResponseDto>.Failure("Product not found", null));
        }

        return Ok(ApiResponseDto<ProductResponseDto>.Success(result, "Product retrieved successfully"));
    }

    /// <summary>
    /// Get all products
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ProductResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductResponseDto>>>> GetAllProducts()
    {
        var result = await _productService.GetAllProductsAsync();
        return Ok(ApiResponseDto<IEnumerable<ProductResponseDto>>.Success(result, "Products retrieved successfully"));
    }

    /// <summary>
    /// Get products by category
    /// </summary>
    [HttpGet("category/{category}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ProductResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductResponseDto>>>> GetProductsByCategory(string category)
    {
        var result = await _productService.GetProductsByCategoryAsync(category);
        return Ok(ApiResponseDto<IEnumerable<ProductResponseDto>>.Success(result, "Products retrieved successfully"));
    }

    /// <summary>
    /// Get low stock products
    /// </summary>
    [HttpGet("low-stock")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ProductResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductResponseDto>>>> GetLowStockProducts()
    {
        var result = await _productService.GetLowStockProductsAsync();
        return Ok(ApiResponseDto<IEnumerable<ProductResponseDto>>.Success(result, "Low stock products retrieved successfully"));
    }

    /// <summary>
    /// Search products
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ProductResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductResponseDto>>>> SearchProducts([FromQuery] string term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return Ok(ApiResponseDto<IEnumerable<ProductResponseDto>>.Failure("Search term is required", null));
        }

        var result = await _productService.SearchProductsAsync(term);
        return Ok(ApiResponseDto<IEnumerable<ProductResponseDto>>.Success(result, "Search completed successfully"));
    }

    /// <summary>
    /// Update product
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<ProductResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<ProductResponseDto>>> UpdateProduct(Guid id, [FromBody] UpdateProductRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<ProductResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _productService.UpdateProductAsync(id, request);
        return Ok(ApiResponseDto<ProductResponseDto>.Success(result, "Product updated successfully"));
    }

    /// <summary>
    /// Update product stock
    /// </summary>
    [HttpPut("{id}/stock")]
    [ProducesResponseType(typeof(ApiResponseDto<ProductResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<ProductResponseDto>>> UpdateStock(Guid id, [FromBody] int quantity)
    {
        var result = await _productService.UpdateStockAsync(id, quantity);
        return Ok(ApiResponseDto<ProductResponseDto>.Success(result, "Stock updated successfully"));
    }

    /// <summary>
    /// Add stock to product
    /// </summary>
    [HttpPost("{id}/stock")]
    [ProducesResponseType(typeof(ApiResponseDto<ProductResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<ProductResponseDto>>> AddStock(Guid id, [FromBody] int quantity)
    {
        var result = await _productService.AddStockAsync(id, quantity);
        return Ok(ApiResponseDto<ProductResponseDto>.Success(result, "Stock added successfully"));
    }

    /// <summary>
    /// Delete product
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> DeleteProduct(Guid id)
    {
        var deleted = await _productService.DeleteProductAsync(id);
        if (!deleted)
        {
            return Ok(ApiResponseDto<object>.Failure("Product not found", null));
        }

        return Ok(ApiResponseDto<object?>.Success(null, "Product deleted successfully"));
    }
}

