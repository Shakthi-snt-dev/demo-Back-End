using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flowtap_Domain.BoundedContexts.Procurement.Entities;

public class PurchaseOrderLine
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid PurchaseOrderId { get; set; }

    public PurchaseOrder? PurchaseOrder { get; set; }

    [Required]
    public Guid ProductId { get; set; }
    // Note: Product is in Inventory context - reference by ID only, no navigation property

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitCost { get; set; }

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Creates a new purchase order line
    /// </summary>
    public static PurchaseOrderLine Create(Guid purchaseOrderId, Guid productId, int quantity, decimal unitCost)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        if (unitCost < 0)
            throw new ArgumentException("Unit cost cannot be negative", nameof(unitCost));

        return new PurchaseOrderLine
        {
            Id = Guid.NewGuid(),
            PurchaseOrderId = purchaseOrderId,
            ProductId = productId,
            Quantity = quantity,
            UnitCost = unitCost
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

    /// <summary>
    /// Updates the unit cost
    /// </summary>
    public void UpdateUnitCost(decimal unitCost)
    {
        if (unitCost < 0)
            throw new ArgumentException("Unit cost cannot be negative", nameof(unitCost));

        UnitCost = unitCost;
    }

    /// <summary>
    /// Calculates the total cost of the line
    /// </summary>
    public decimal GetTotalCost()
    {
        return Quantity * UnitCost;
    }
}

