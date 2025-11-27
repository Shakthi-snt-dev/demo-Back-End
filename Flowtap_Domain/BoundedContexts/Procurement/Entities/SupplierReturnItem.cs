using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Procurement.Entities;

public class SupplierReturnItem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid SupplierReturnId { get; set; }

    public SupplierReturn? SupplierReturn { get; set; }

    public Guid InventoryItemId { get; set; }
    // Note: InventoryItem is in Inventory context - reference by ID only, no navigation property

    public int Quantity { get; set; }

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Creates a new supplier return item
    /// </summary>
    public static SupplierReturnItem Create(Guid supplierReturnId, Guid inventoryItemId, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        return new SupplierReturnItem
        {
            Id = Guid.NewGuid(),
            SupplierReturnId = supplierReturnId,
            InventoryItemId = inventoryItemId,
            Quantity = quantity
        };
    }

    /// <summary>
    /// Updates the quantity
    /// </summary>
    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        Quantity = quantity;
    }
}

