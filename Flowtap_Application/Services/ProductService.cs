using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Inventory.Entities;
using Flowtap_Domain.BoundedContexts.Inventory.Interfaces;
using Flowtap_Domain.DtoModel;
using Flowtap_Domain.Exceptions;

namespace Flowtap_Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        IProductRepository productRepository,
        ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<ProductResponseDto> CreateProductAsync(CreateProductRequestDto request)
    {
        // Check if SKU already exists for this store
        var existingProduct = await _productRepository.GetBySkuAsync(request.SKU);
        if (existingProduct != null && existingProduct.StoreId == request.StoreId)
            throw new Flowtap_Domain.Exceptions.InvalidOperationException(
                $"Product with SKU '{request.SKU}' already exists for this store",
                "Product",
                new Dictionary<string, string> { { "SKU", request.SKU }, { "StoreId", request.StoreId.ToString() } });

        var product = new Product
        {
            Id = Guid.NewGuid(),
            StoreId = request.StoreId,
            SKU = request.SKU,
            Name = request.Name,
            Category = request.Category,
            Stock = request.Stock,
            MinStock = request.MinStock,
            Price = request.Price,
            Cost = request.Cost,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _productRepository.CreateAsync(product);
        return MapToDto(created);
    }

    public async Task<ProductResponseDto> GetProductByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            throw new EntityNotFoundException("Product", id);

        return MapToDto(product);
    }

    public async Task<ProductResponseDto?> GetProductBySkuAsync(string sku)
    {
        var product = await _productRepository.GetBySkuAsync(sku);
        return product != null ? MapToDto(product) : null;
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetProductsByCategoryAsync(string category)
    {
        var products = await _productRepository.GetByCategoryAsync(category);
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetLowStockProductsAsync()
    {
        var products = await _productRepository.GetLowStockItemsAsync();
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductResponseDto>> SearchProductsAsync(string searchTerm)
    {
        var products = await _productRepository.SearchAsync(searchTerm);
        return products.Select(MapToDto);
    }

    public async Task<ProductResponseDto> UpdateProductAsync(Guid id, UpdateProductRequestDto request)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            throw new EntityNotFoundException("Product", id);

        if (!string.IsNullOrWhiteSpace(request.Name))
            product.Name = request.Name;

        if (!string.IsNullOrWhiteSpace(request.Category))
            product.Category = request.Category;

        if (request.Stock.HasValue)
            product.UpdateStock(request.Stock.Value);

        if (request.MinStock.HasValue)
            product.UpdateMinStock(request.MinStock.Value);

        if (request.Price.HasValue)
            product.UpdatePrice(request.Price.Value);

        if (request.Cost.HasValue)
            product.UpdateCost(request.Cost.Value);

        var updated = await _productRepository.UpdateAsync(product);
        return MapToDto(updated);
    }

    public async Task<ProductResponseDto> UpdateStockAsync(Guid id, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            throw new EntityNotFoundException("Product", id);

        product.UpdateStock(quantity);
        var updated = await _productRepository.UpdateAsync(product);
        return MapToDto(updated);
    }

    public async Task<ProductResponseDto> AddStockAsync(Guid id, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            throw new EntityNotFoundException("Product", id);

        product.AddStock(quantity);
        var updated = await _productRepository.UpdateAsync(product);
        return MapToDto(updated);
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        return await _productRepository.DeleteAsync(id);
    }

    private static ProductResponseDto MapToDto(Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            StoreId = product.StoreId,
            SKU = product.SKU,
            Name = product.Name,
            Category = product.Category,
            Stock = product.Stock,
            MinStock = product.MinStock,
            Price = product.Price,
            Cost = product.Cost,
            IsLowStock = product.IsLowStock(),
            IsInStock = product.IsInStock(),
            ProfitMargin = product.GetProfitMargin(),
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
}

