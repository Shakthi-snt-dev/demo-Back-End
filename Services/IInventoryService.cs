using FlowTap.Api.Models;

namespace FlowTap.Api.Services;

public interface IInventoryService
{
    Task<List<ProductDto>> GetProductsAsync(string? category, string? search);
    Task<Product> CreateProductAsync(CreateProductRequest request);
    Task<Product> UpdateProductAsync(Guid id, UpdateProductRequest request);
    Task<bool> DeleteProductAsync(Guid id);
}

public class CreateProductRequest
{
    public string SKU { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public decimal Cost { get; set; }
    public decimal RetailPrice { get; set; }
    public int Stock { get; set; }
    public int LowStockAlert { get; set; } = 10;
    public string? Supplier { get; set; }
    public bool SyncShopify { get; set; } = false;
    public bool SyncWooCommerce { get; set; } = false;
}

public class UpdateProductRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public decimal? Cost { get; set; }
    public decimal? RetailPrice { get; set; }
    public int? Stock { get; set; }
    public int? LowStockAlert { get; set; }
    public string? Supplier { get; set; }
    public bool? SyncShopify { get; set; }
    public bool? SyncWooCommerce { get; set; }
}

public class ProductDto
{
    public Guid Id { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public decimal Cost { get; set; }
    public decimal RetailPrice { get; set; }
    public int Stock { get; set; }
    public int LowStockAlert { get; set; }
    public string? Supplier { get; set; }
    public bool SyncShopify { get; set; }
    public bool SyncWooCommerce { get; set; }
    public DateTime CreatedAt { get; set; }
}

