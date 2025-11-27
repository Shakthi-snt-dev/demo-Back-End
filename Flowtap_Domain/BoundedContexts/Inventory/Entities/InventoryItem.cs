using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flowtap_Domain.BoundedContexts.Inventory.Entities;

public class InventoryItem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ProductId { get; set; }

    public Product? Product { get; set; }

    [Required]
    public Guid StoreId { get; set; }
    // Note: Store is in Store context - reference by ID only, no navigation property

    public int QuantityOnHand { get; set; }

    public int QuantityReserved { get; set; }

    public int ReorderLevel { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }

    public ICollection<SerialNumber> Serials { get; set; } = new List<SerialNumber>();

    public ICollection<InventoryTransaction> Transactions { get; set; } = new List<InventoryTransaction>();

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the location of the inventory item
    /// </summary>
    public void UpdateLocation(string? location)
    {
        if (location != null && location.Length > 200)
            throw new ArgumentException("Location cannot exceed 200 characters", nameof(location));

        Location = location;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the reorder level
    /// </summary>
    public void UpdateReorderLevel(int reorderLevel)
    {
        if (reorderLevel < 0)
            throw new ArgumentException("Reorder level cannot be negative", nameof(reorderLevel));

        ReorderLevel = reorderLevel;
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

    /// <summary>
    /// Checks if the item is below reorder level
    /// </summary>
    public bool IsBelowReorderLevel()
    {
        return QuantityOnHand <= ReorderLevel;
    }

    /// <summary>
    /// Gets the available quantity (on hand minus reserved)
    /// </summary>
    public int GetQuantityAvailable()
    {
        return QuantityOnHand - QuantityReserved;
    }

    /// <summary>
    /// Reserves quantity
    /// </summary>
    public void ReserveQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidOperationException("Quantity must be greater than zero");
        if (GetQuantityAvailable() < quantity)
            throw new InvalidOperationException("Insufficient available quantity");

        QuantityReserved += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Releases reserved quantity
    /// </summary>
    public void ReleaseReservedQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidOperationException("Quantity must be greater than zero");
        if (QuantityReserved < quantity)
            throw new InvalidOperationException("Cannot release more than reserved");

        QuantityReserved -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }
}

