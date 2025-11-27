using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Presentation.Attributes;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Product SubCategories Controller - Manage product subcategories
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AppUserOrOwner]
public class ProductSubCategoriesController : ControllerBase
{
    private readonly IProductSubCategoryService _subCategoryService;
    private readonly ILogger<ProductSubCategoriesController> _logger;

    public ProductSubCategoriesController(
        IProductSubCategoryService subCategoryService,
        ILogger<ProductSubCategoriesController> logger)
    {
        _subCategoryService = subCategoryService;
        _logger = logger;
    }

    /// <summary>
    /// Get all product subcategories
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ProductSubCategoryResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductSubCategoryResponseDto>>>> GetAllProductSubCategories()
    {
        try
        {
            var subCategories = await _subCategoryService.GetAllProductSubCategoriesAsync();
            return Ok(ApiResponseDto<IEnumerable<ProductSubCategoryResponseDto>>.Success(subCategories, "Product subcategories retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product subcategories");
            return BadRequest(ApiResponseDto<IEnumerable<ProductSubCategoryResponseDto>>.Failure("Error retrieving product subcategories", null));
        }
    }

    /// <summary>
    /// Get product subcategory by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<ProductSubCategoryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<ProductSubCategoryResponseDto>>> GetProductSubCategoryById(Guid id)
    {
        try
        {
            var subCategory = await _subCategoryService.GetProductSubCategoryByIdAsync(id);
            return Ok(ApiResponseDto<ProductSubCategoryResponseDto>.Success(subCategory, "Product subcategory retrieved successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<ProductSubCategoryResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product subcategory {SubCategoryId}", id);
            return BadRequest(ApiResponseDto<ProductSubCategoryResponseDto>.Failure("Error retrieving product subcategory", null));
        }
    }

    /// <summary>
    /// Get product subcategories by category ID
    /// </summary>
    [HttpGet("category/{categoryId}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ProductSubCategoryResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductSubCategoryResponseDto>>>> GetProductSubCategoriesByCategory(Guid categoryId)
    {
        try
        {
            var subCategories = await _subCategoryService.GetProductSubCategoriesByCategoryIdAsync(categoryId);
            return Ok(ApiResponseDto<IEnumerable<ProductSubCategoryResponseDto>>.Success(subCategories, "Product subcategories retrieved successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<IEnumerable<ProductSubCategoryResponseDto>>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product subcategories for category {CategoryId}", categoryId);
            return BadRequest(ApiResponseDto<IEnumerable<ProductSubCategoryResponseDto>>.Failure("Error retrieving product subcategories", null));
        }
    }

    /// <summary>
    /// Create a new product subcategory
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<ProductSubCategoryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponseDto<ProductSubCategoryResponseDto>>> CreateProductSubCategory([FromBody] CreateProductSubCategoryRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<ProductSubCategoryResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var subCategory = await _subCategoryService.CreateProductSubCategoryAsync(request);
            return Ok(ApiResponseDto<ProductSubCategoryResponseDto>.Success(subCategory, "Product subcategory created successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<ProductSubCategoryResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product subcategory");
            return BadRequest(ApiResponseDto<ProductSubCategoryResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Update product subcategory
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<ProductSubCategoryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<ProductSubCategoryResponseDto>>> UpdateProductSubCategory(Guid id, [FromBody] UpdateProductSubCategoryRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<ProductSubCategoryResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var subCategory = await _subCategoryService.UpdateProductSubCategoryAsync(id, request);
            return Ok(ApiResponseDto<ProductSubCategoryResponseDto>.Success(subCategory, "Product subcategory updated successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<ProductSubCategoryResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product subcategory {SubCategoryId}", id);
            return BadRequest(ApiResponseDto<ProductSubCategoryResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Delete product subcategory
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteProductSubCategory(Guid id)
    {
        try
        {
            var result = await _subCategoryService.DeleteProductSubCategoryAsync(id);
            return Ok(ApiResponseDto<bool>.Success(result, "Product subcategory deleted successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<bool>.Failure(ex.Message, false));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product subcategory {SubCategoryId}", id);
            return BadRequest(ApiResponseDto<bool>.Failure("Error deleting product subcategory", false));
        }
    }
}

