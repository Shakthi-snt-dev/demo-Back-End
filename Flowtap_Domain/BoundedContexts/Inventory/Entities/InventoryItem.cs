using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flowtap_Domain.BoundedContexts.Inventory.Entities;

public class InventoryItem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid StoreId { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? SKU { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal CostPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal SellPrice { get; set; }

    public int QuantityOnHand { get; set; } = 0;

    // Note: PartsUsed are in Service context - reference by InventoryItemId only, no navigation property

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the inventory item information
    /// </summary>
    public void UpdateItemInfo(string name, string? sku, decimal costPrice, decimal sellPrice)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (costPrice < 0)
            throw new ArgumentException("Cost price cannot be negative", nameof(costPrice));
        if (sellPrice < 0)
            throw new ArgumentException("Sell price cannot be negative", nameof(sellPrice));

        Name = name;
        SKU = sku;
        CostPrice = costPrice;
        SellPrice = sellPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the quantity on hand
    /// </summary>
    public void UpdateQuantity(int quantity)
    {
        if (quantity < 0)
            throw new InvalidOperationException("Quantity cannot be negative");

        QuantityOnHand = quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds quantity to inventory
    /// </summary>
    public void AddQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidOperationException("Quantity must be greater than zero");

        QuantityOnHand += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes quantity from inventory
    /// </summary>
    public void RemoveQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidOperationException("Quantity must be greater than zero");
        if (QuantityOnHand < quantity)
            throw new InvalidOperationException("Insufficient quantity on hand");

        QuantityOnHand -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the cost price
    /// </summary>
    public void UpdateCostPrice(decimal costPrice)
    {
        if (costPrice < 0)
            throw new ArgumentException("Cost price cannot be negative", nameof(costPrice));

        CostPrice = costPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the sell price
    /// </summary>
    public void UpdateSellPrice(decimal sellPrice)
    {
        if (sellPrice < 0)
            throw new ArgumentException("Sell price cannot be negative", nameof(sellPrice));

        SellPrice = sellPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Checks if the item is in stock
    /// </summary>
    public bool IsInStock()
    {
        return QuantityOnHand > 0;
    }

    /// <summary>
    /// Checks if the item has sufficient quantity
    /// </summary>
    public bool HasSufficientQuantity(int requiredQuantity)
    {
        return QuantityOnHand >= requiredQuantity;
    }
}

