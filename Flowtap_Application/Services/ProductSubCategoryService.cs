using AutoMapper;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Inventory.Entities;
using Flowtap_Domain.BoundedContexts.Inventory.Interfaces;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class ProductSubCategoryService : IProductSubCategoryService
{
    private readonly IProductSubCategoryRepository _subCategoryRepository;
    private readonly IProductCategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductSubCategoryService> _logger;

    public ProductSubCategoryService(
        IProductSubCategoryRepository subCategoryRepository,
        IProductCategoryRepository categoryRepository,
        IMapper mapper,
        ILogger<ProductSubCategoryService> logger)
    {
        _subCategoryRepository = subCategoryRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductSubCategoryResponseDto> GetProductSubCategoryByIdAsync(Guid id)
    {
        var subCategory = await _subCategoryRepository.GetByIdAsync(id);
        if (subCategory == null)
        {
            throw new EntityNotFoundException("ProductSubCategory", id);
        }

        var dto = _mapper.Map<ProductSubCategoryResponseDto>(subCategory);
        
        // Load category name if available
        if (subCategory.Category != null)
        {
            dto.CategoryName = subCategory.Category.Name;
        }
        else if (subCategory.CategoryId != Guid.Empty)
        {
            var category = await _categoryRepository.GetByIdAsync(subCategory.CategoryId);
            if (category != null)
            {
                dto.CategoryName = category.Name;
            }
        }

        return dto;
    }

    public async Task<IEnumerable<ProductSubCategoryResponseDto>> GetAllProductSubCategoriesAsync()
    {
        var subCategories = await _subCategoryRepository.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<ProductSubCategoryResponseDto>>(subCategories).ToList();

        // Load category names
        foreach (var dto in dtos)
        {
            var subCategory = subCategories.FirstOrDefault(sc => sc.Id == dto.Id);
            if (subCategory?.Category != null)
            {
                dto.CategoryName = subCategory.Category.Name;
            }
            else if (dto.CategoryId != Guid.Empty)
            {
                var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
                if (category != null)
                {
                    dto.CategoryName = category.Name;
                }
            }
        }

        return dtos;
    }

    public async Task<IEnumerable<ProductSubCategoryResponseDto>> GetProductSubCategoriesByCategoryIdAsync(Guid categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category == null)
        {
            throw new EntityNotFoundException("ProductCategory", categoryId);
        }

        var subCategories = await _subCategoryRepository.GetByCategoryIdAsync(categoryId);
        var dtos = _mapper.Map<IEnumerable<ProductSubCategoryResponseDto>>(subCategories).ToList();

        // Set category name
        foreach (var dto in dtos)
        {
            dto.CategoryName = category.Name;
        }

        return dtos;
    }

    public async Task<ProductSubCategoryResponseDto> CreateProductSubCategoryAsync(CreateProductSubCategoryRequestDto request)
    {
        // Verify category exists
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
        if (category == null)
        {
            throw new EntityNotFoundException("ProductCategory", request.CategoryId);
        }

        var subCategory = new ProductSubCategory
        {
            Id = Guid.NewGuid(),
            CategoryId = request.CategoryId,
            Name = request.Name
        };

        var createdSubCategory = await _subCategoryRepository.CreateAsync(subCategory);
        _logger.LogInformation("Created product subcategory {SubCategoryId}", createdSubCategory.Id);

        var dto = _mapper.Map<ProductSubCategoryResponseDto>(createdSubCategory);
        dto.CategoryName = category.Name;

        return dto;
    }

    public async Task<ProductSubCategoryResponseDto> UpdateProductSubCategoryAsync(Guid id, UpdateProductSubCategoryRequestDto request)
    {
        var subCategory = await _subCategoryRepository.GetByIdAsync(id);
        if (subCategory == null)
        {
            throw new EntityNotFoundException("ProductSubCategory", id);
        }

        // Verify category exists
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
        if (category == null)
        {
            throw new EntityNotFoundException("ProductCategory", request.CategoryId);
        }

        subCategory.UpdateName(request.Name);
        subCategory.ChangeCategory(request.CategoryId);
        
        await _subCategoryRepository.UpdateAsync(subCategory);
        _logger.LogInformation("Updated product subcategory {SubCategoryId}", id);

        var dto = _mapper.Map<ProductSubCategoryResponseDto>(subCategory);
        dto.CategoryName = category.Name;

        return dto;
    }

    public async Task<bool> DeleteProductSubCategoryAsync(Guid id)
    {
        var subCategory = await _subCategoryRepository.GetByIdAsync(id);
        if (subCategory == null)
        {
            throw new EntityNotFoundException("ProductSubCategory", id);
        }

        var result = await _subCategoryRepository.DeleteAsync(id);
        _logger.LogInformation("Deleted product subcategory {SubCategoryId}", id);

        return result;
    }
}

