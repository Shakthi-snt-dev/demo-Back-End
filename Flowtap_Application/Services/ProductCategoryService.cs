using AutoMapper;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Inventory.Entities;
using Flowtap_Domain.BoundedContexts.Inventory.Interfaces;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class ProductCategoryService : IProductCategoryService
{
    private readonly IProductCategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductCategoryService> _logger;

    public ProductCategoryService(
        IProductCategoryRepository categoryRepository,
        IMapper mapper,
        ILogger<ProductCategoryService> logger)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductCategoryResponseDto> GetProductCategoryByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            throw new EntityNotFoundException("ProductCategory", id);
        }

        return _mapper.Map<ProductCategoryResponseDto>(category);
    }

    public async Task<IEnumerable<ProductCategoryResponseDto>> GetAllProductCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProductCategoryResponseDto>>(categories);
    }

    public async Task<ProductCategoryResponseDto> CreateProductCategoryAsync(CreateProductCategoryRequestDto request)
    {
        // Check if category with same name already exists
        var existingCategory = await _categoryRepository.GetByNameAsync(request.Name);
        if (existingCategory != null)
        {
            throw new System.InvalidOperationException($"Product category with name '{request.Name}' already exists");
        }

        var category = new ProductCategory
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        var createdCategory = await _categoryRepository.CreateAsync(category);
        _logger.LogInformation("Created product category {CategoryId}", createdCategory.Id);

        return _mapper.Map<ProductCategoryResponseDto>(createdCategory);
    }

    public async Task<ProductCategoryResponseDto> UpdateProductCategoryAsync(Guid id, UpdateProductCategoryRequestDto request)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            throw new EntityNotFoundException("ProductCategory", id);
        }

        // Check if another category with same name exists
        var existingCategory = await _categoryRepository.GetByNameAsync(request.Name);
        if (existingCategory != null && existingCategory.Id != id)
        {
            throw new System.InvalidOperationException($"Product category with name '{request.Name}' already exists");
        }

        category.UpdateName(request.Name);
        await _categoryRepository.UpdateAsync(category);
        _logger.LogInformation("Updated product category {CategoryId}", id);

        return _mapper.Map<ProductCategoryResponseDto>(category);
    }

    public async Task<bool> DeleteProductCategoryAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            throw new EntityNotFoundException("ProductCategory", id);
        }

        var result = await _categoryRepository.DeleteAsync(id);
        _logger.LogInformation("Deleted product category {CategoryId}", id);

        return result;
    }
}

