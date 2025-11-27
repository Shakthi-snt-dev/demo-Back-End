using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Presentation.Attributes;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Products Controller - Manage products
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AppUserOrOwner]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        IProductService productService,
        ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Get product by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<ProductResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<ProductResponseDto>>> GetProductById(Guid id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(ApiResponseDto<ProductResponseDto>.Success(product, "Product retrieved successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<ProductResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product {ProductId}", id);
            return BadRequest(ApiResponseDto<ProductResponseDto>.Failure("Error retrieving product", null));
        }
    }

    /// <summary>
    /// Get all products
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ProductResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductResponseDto>>>> GetAllProducts()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(ApiResponseDto<IEnumerable<ProductResponseDto>>.Success(products, "Products retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products");
            return BadRequest(ApiResponseDto<IEnumerable<ProductResponseDto>>.Failure("Error retrieving products", null));
        }
    }

    /// <summary>
    /// Search products
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ProductResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductResponseDto>>>> SearchProducts([FromQuery] string searchTerm)
    {
        try
        {
            var products = await _productService.SearchProductsAsync(searchTerm);
            return Ok(ApiResponseDto<IEnumerable<ProductResponseDto>>.Success(products, "Products retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching products");
            return BadRequest(ApiResponseDto<IEnumerable<ProductResponseDto>>.Failure("Error searching products", null));
        }
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<ProductResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponseDto<ProductResponseDto>>> CreateProduct([FromBody] CreateProductRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<ProductResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var product = await _productService.CreateProductAsync(request);
            return Ok(ApiResponseDto<ProductResponseDto>.Success(product, "Product created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            return BadRequest(ApiResponseDto<ProductResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Update product
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<ProductResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<ProductResponseDto>>> UpdateProduct(Guid id, [FromBody] UpdateProductRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<ProductResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var product = await _productService.UpdateProductAsync(id, request);
            return Ok(ApiResponseDto<ProductResponseDto>.Success(product, "Product updated successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<ProductResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product {ProductId}", id);
            return BadRequest(ApiResponseDto<ProductResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Delete product
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteProduct(Guid id)
    {
        try
        {
            var result = await _productService.DeleteProductAsync(id);
            return Ok(ApiResponseDto<bool>.Success(result, "Product deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product {ProductId}", id);
            return BadRequest(ApiResponseDto<bool>.Failure("Error deleting product", false));
        }
    }
}

