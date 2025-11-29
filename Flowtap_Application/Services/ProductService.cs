using AutoMapper;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Inventory.Entities;
using Flowtap_Domain.BoundedContexts.Inventory.Interfaces;
using Flowtap_Domain.BoundedContexts.Store.Interfaces;
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
    private readonly IInventoryItemRepository _inventoryItemRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        IProductRepository productRepository,
        IProductCategoryRepository categoryRepository,
        IProductSubCategoryRepository subCategoryRepository,
        IInventoryItemRepository inventoryItemRepository,
        IStoreRepository storeRepository,
        IMapper mapper,
        ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _subCategoryRepository = subCategoryRepository;
        _inventoryItemRepository = inventoryItemRepository;
        _storeRepository = storeRepository;
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
        // Set IsActive from request (mapper sets default to true, but we allow override)
        product.IsActive = request.IsActive;

        var createdProduct = await _productRepository.CreateAsync(product);
        _logger.LogInformation("Created product {ProductId}", createdProduct.Id);

        // Create InventoryItem if stock fields are provided
        if (request.OnHandQty.HasValue || request.StockWarning.HasValue || request.ReorderLevel.HasValue)
        {
            if (!request.StoreId.HasValue)
            {
                throw new ArgumentException("StoreId is required when OnHandQty, StockWarning, or ReorderLevel are provided");
            }

            // Validate store exists
            var store = await _storeRepository.GetByIdAsync(request.StoreId.Value);
            if (store == null)
            {
                throw new EntityNotFoundException("Store", request.StoreId.Value);
            }

            // Check if inventory item already exists
            var existingInventoryItem = await _inventoryItemRepository.GetByProductIdAndStoreIdAsync(
                createdProduct.Id, request.StoreId.Value);
            
            if (existingInventoryItem != null)
            {
                // Update existing inventory item
                if (request.OnHandQty.HasValue)
                    existingInventoryItem.UpdateQuantity(request.OnHandQty.Value);
                if (request.ReorderLevel.HasValue)
                    existingInventoryItem.UpdateReorderLevel(request.ReorderLevel.Value);
                else if (request.StockWarning.HasValue)
                    existingInventoryItem.UpdateStockWarning(request.StockWarning.Value);
                
                await _inventoryItemRepository.UpdateAsync(existingInventoryItem);
                _logger.LogInformation("Updated inventory item for product {ProductId} in store {StoreId}", 
                    createdProduct.Id, request.StoreId.Value);
            }
            else
            {
                // Create new inventory item
                var inventoryItem = new InventoryItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = createdProduct.Id,
                    StoreId = request.StoreId.Value,
                    QuantityOnHand = request.OnHandQty ?? 0,
                    ReorderLevel = request.ReorderLevel ?? request.StockWarning ?? 0,
                    UpdatedAt = DateTime.UtcNow
                };

                await _inventoryItemRepository.CreateAsync(inventoryItem);
                _logger.LogInformation("Created inventory item for product {ProductId} in store {StoreId}", 
                    createdProduct.Id, request.StoreId.Value);
            }
        }

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

        if (request.ProductType.HasValue)
            product.SetProductType(request.ProductType.Value);

        // Update IsActive if provided
        if (request.IsActive.HasValue)
        {
            product.IsActive = request.IsActive.Value;
        }

        var updatedProduct = await _productRepository.UpdateAsync(product);

        // Update InventoryItem if stock fields are provided
        if (request.OnHandQty.HasValue || request.StockWarning.HasValue || request.ReorderLevel.HasValue)
        {
            if (!request.StoreId.HasValue)
            {
                throw new ArgumentException("StoreId is required when OnHandQty, StockWarning, or ReorderLevel are provided");
            }

            // Validate store exists
            var store = await _storeRepository.GetByIdAsync(request.StoreId.Value);
            if (store == null)
            {
                throw new EntityNotFoundException("Store", request.StoreId.Value);
            }

            // Get or create inventory item
            var inventoryItem = await _inventoryItemRepository.GetByProductIdAndStoreIdAsync(
                id, request.StoreId.Value);

            if (inventoryItem == null)
            {
                // Create new inventory item if it doesn't exist
                inventoryItem = new InventoryItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = id,
                    StoreId = request.StoreId.Value,
                    QuantityOnHand = request.OnHandQty ?? 0,
                    ReorderLevel = request.ReorderLevel ?? request.StockWarning ?? 0,
                    UpdatedAt = DateTime.UtcNow
                };

                await _inventoryItemRepository.CreateAsync(inventoryItem);
                _logger.LogInformation("Created inventory item for product {ProductId} in store {StoreId}", 
                    id, request.StoreId.Value);
            }
            else
            {
                // Update existing inventory item
                if (request.OnHandQty.HasValue)
                    inventoryItem.UpdateQuantity(request.OnHandQty.Value);
                if (request.ReorderLevel.HasValue)
                    inventoryItem.UpdateReorderLevel(request.ReorderLevel.Value);
                else if (request.StockWarning.HasValue)
                    inventoryItem.UpdateStockWarning(request.StockWarning.Value);

                await _inventoryItemRepository.UpdateAsync(inventoryItem);
                _logger.LogInformation("Updated inventory item for product {ProductId} in store {StoreId}", 
                    id, request.StoreId.Value);
            }
        }

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

        // Calculate total quantity on hand across all stores
        var inventoryItems = await _inventoryItemRepository.GetByProductIdAsync(product.Id);
        dto.OnHandQty = inventoryItems.Sum(item => item.QuantityOnHand);
        
        // Calculate stock warning (minimum reorder level across all stores, or 0 if no inventory items)
        dto.StockWarning = inventoryItems.Any() 
            ? inventoryItems.Min(item => item.ReorderLevel) 
            : 0;

        dto.ProfitMargin = product.GetProfitMargin();
        dto.ProfitAmount = product.GetProfitAmount();

        return dto;
    }
}

