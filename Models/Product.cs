namespace FlowTap.Api.Models;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
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
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

