using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Presentation.Attributes;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Product Categories Controller - Manage product categories
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AppUserOrOwner]
public class ProductCategoriesController : ControllerBase
{
    private readonly IProductCategoryService _categoryService;
    private readonly ILogger<ProductCategoriesController> _logger;

    public ProductCategoriesController(
        IProductCategoryService categoryService,
        ILogger<ProductCategoriesController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    /// <summary>
    /// Get all product categories
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ProductCategoryResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductCategoryResponseDto>>>> GetAllProductCategories()
    {
        try
        {
            var categories = await _categoryService.GetAllProductCategoriesAsync();
            return Ok(ApiResponseDto<IEnumerable<ProductCategoryResponseDto>>.Success(categories, "Product categories retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product categories");
            return BadRequest(ApiResponseDto<IEnumerable<ProductCategoryResponseDto>>.Failure("Error retrieving product categories", null));
        }
    }

    /// <summary>
    /// Get product category by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<ProductCategoryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<ProductCategoryResponseDto>>> GetProductCategoryById(Guid id)
    {
        try
        {
            var category = await _categoryService.GetProductCategoryByIdAsync(id);
            return Ok(ApiResponseDto<ProductCategoryResponseDto>.Success(category, "Product category retrieved successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<ProductCategoryResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product category {CategoryId}", id);
            return BadRequest(ApiResponseDto<ProductCategoryResponseDto>.Failure("Error retrieving product category", null));
        }
    }

    /// <summary>
    /// Create a new product category
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<ProductCategoryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponseDto<ProductCategoryResponseDto>>> CreateProductCategory([FromBody] CreateProductCategoryRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<ProductCategoryResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var category = await _categoryService.CreateProductCategoryAsync(request);
            return Ok(ApiResponseDto<ProductCategoryResponseDto>.Success(category, "Product category created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product category");
            return BadRequest(ApiResponseDto<ProductCategoryResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Update product category
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<ProductCategoryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<ProductCategoryResponseDto>>> UpdateProductCategory(Guid id, [FromBody] UpdateProductCategoryRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<ProductCategoryResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var category = await _categoryService.UpdateProductCategoryAsync(id, request);
            return Ok(ApiResponseDto<ProductCategoryResponseDto>.Success(category, "Product category updated successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<ProductCategoryResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product category {CategoryId}", id);
            return BadRequest(ApiResponseDto<ProductCategoryResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Delete product category
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteProductCategory(Guid id)
    {
        try
        {
            var result = await _categoryService.DeleteProductCategoryAsync(id);
            return Ok(ApiResponseDto<bool>.Success(result, "Product category deleted successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<bool>.Failure(ex.Message, false));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product category {CategoryId}", id);
            return BadRequest(ApiResponseDto<bool>.Failure("Error deleting product category", false));
        }
    }
}

