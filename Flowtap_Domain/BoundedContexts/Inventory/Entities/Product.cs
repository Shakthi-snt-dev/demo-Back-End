using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flowtap_Domain.BoundedContexts.Inventory.Entities;

public class Product
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid StoreId { get; set; }

    [Required, MaxLength(100)]
    public string SKU { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Category { get; set; }

    public int Stock { get; set; } = 0;

    public int MinStock { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Cost { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Domain methods
    public void UpdateStock(int quantity)
    {
        if (quantity < 0)
            throw new InvalidOperationException("Stock quantity cannot be negative");

        Stock = quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidOperationException("Quantity must be greater than zero");

        Stock += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidOperationException("Quantity must be greater than zero");

        if (Stock < quantity)
            throw new InvalidOperationException("Insufficient stock");

        Stock -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePrice(decimal price)
    {
        if (price < 0)
            throw new InvalidOperationException("Price cannot be negative");

        Price = price;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCost(decimal cost)
    {
        if (cost < 0)
            throw new InvalidOperationException("Cost cannot be negative");

        Cost = cost;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateMinStock(int minStock)
    {
        if (minStock < 0)
            throw new InvalidOperationException("Minimum stock cannot be negative");

        MinStock = minStock;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsLowStock()
    {
        return Stock <= MinStock;
    }

    public bool IsInStock()
    {
        return Stock > 0;
    }

    public bool HasSufficientStock(int quantity)
    {
        return Stock >= quantity;
    }

    public decimal GetProfitMargin()
    {
        if (Price == 0)
            return 0;

        return ((Price - Cost) / Price) * 100;
    }
}

