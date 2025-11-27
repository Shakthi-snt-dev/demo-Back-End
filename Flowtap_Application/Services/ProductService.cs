using AutoMapper;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Inventory.Entities;
using Flowtap_Domain.BoundedContexts.Inventory.Interfaces;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _categoryRepository;
    private readonly IProductSubCategoryRepository _subCategoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        IProductRepository productRepository,
        IProductCategoryRepository categoryRepository,
        IProductSubCategoryRepository subCategoryRepository,
        IMapper mapper,
        ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _subCategoryRepository = subCategoryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductResponseDto> CreateProductAsync(CreateProductRequestDto request)
    {
        if (!string.IsNullOrWhiteSpace(request.SKU))
        {
            var existingProduct = await _productRepository.GetBySkuAsync(request.SKU);
            if (existingProduct != null)
            {
                throw new System.InvalidOperationException($"Product with SKU {request.SKU} already exists");
            }
        }

        var product = _mapper.Map<Product>(request);
        product.Id = Guid.NewGuid();
        product.CreatedAt = DateTime.UtcNow;

        var createdProduct = await _productRepository.CreateAsync(product);
        _logger.LogInformation("Created product {ProductId}", createdProduct.Id);

        return await MapToResponseDto(createdProduct);
    }

    public async Task<ProductResponseDto> GetProductByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new EntityNotFoundException("Product", id);
        }

        return await MapToResponseDto(product);
    }

    public async Task<ProductResponseDto?> GetProductBySkuAsync(string sku)
    {
        var product = await _productRepository.GetBySkuAsync(sku);
        if (product == null)
            return null;

        return await MapToResponseDto(product);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        var result = new List<ProductResponseDto>();

        foreach (var product in products)
        {
            result.Add(await MapToResponseDto(product));
        }

        return result;
    }

    public async Task<IEnumerable<ProductResponseDto>> GetProductsByCategoryAsync(string category)
    {
        // This method signature needs to be updated to use categoryId
        var products = await _productRepository.GetAllAsync();
        return products.Select(p => MapToResponseDto(p).Result);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetLowStockProductsAsync()
    {
        // This would need to check inventory items, not products directly
        var products = await _productRepository.GetAllAsync();
        return products.Select(p => MapToResponseDto(p).Result);
    }

    public async Task<IEnumerable<ProductResponseDto>> SearchProductsAsync(string searchTerm)
    {
        var products = await _productRepository.SearchAsync(searchTerm);
        var result = new List<ProductResponseDto>();

        foreach (var product in products)
        {
            result.Add(await MapToResponseDto(product));
        }

        return result;
    }

    public async Task<ProductResponseDto> UpdateProductAsync(Guid id, UpdateProductRequestDto request)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new EntityNotFoundException("Product", id);
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
            product.UpdateProductInfo(
                request.Name, 
                request.SKU, 
                request.Brand, 
                request.Description, 
                request.CostPrice ?? product.CostPrice, 
                request.SalePrice ?? product.SalePrice,
                request.Condition ?? product.Condition,
                request.InventoryValuationMethod ?? product.InventoryValuationMethod,
                request.MinimumPrice ?? product.MinimumPrice,
                request.TaxClass ?? product.TaxClass,
                request.ShowOnPOS ?? product.ShowOnPOS
            );

        if (request.CategoryId.HasValue || request.SubCategoryId.HasValue)
            product.SetCategory(request.CategoryId, request.SubCategoryId);

        if (request.SupplierId.HasValue)
            product.SetSupplier(request.SupplierId);

        if (request.TrackSerials.HasValue)
            product.SetTrackSerials(request.TrackSerials.Value);

        // ProductType is set via mapper, but we can also set it explicitly if needed
        // IsActive is set via mapper

        var updatedProduct = await _productRepository.UpdateAsync(product);
        return await MapToResponseDto(updatedProduct);
    }

    public async Task<ProductResponseDto> UpdateStockAsync(Guid id, int quantity)
    {
        // Stock is now managed through InventoryItem, not Product
        throw new NotImplementedException("Stock is managed through InventoryItem. Use InventoryItemService instead.");
    }

    public async Task<ProductResponseDto> AddStockAsync(Guid id, int quantity)
    {
        throw new NotImplementedException("Stock is managed through InventoryItem. Use InventoryItemService instead.");
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        return await _productRepository.DeleteAsync(id);
    }

    private async Task<ProductResponseDto> MapToResponseDto(Product product)
    {
        var dto = _mapper.Map<ProductResponseDto>(product);
        
        if (product.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(product.CategoryId.Value);
            dto.CategoryName = category?.Name;
        }

        if (product.SubCategoryId.HasValue)
        {
            var subCategory = await _subCategoryRepository.GetByIdAsync(product.SubCategoryId.Value);
            dto.SubCategoryName = subCategory?.Name;
        }

        dto.ProfitMargin = product.GetProfitMargin();
        dto.ProfitAmount = product.GetProfitAmount();

        return dto;
    }
}

