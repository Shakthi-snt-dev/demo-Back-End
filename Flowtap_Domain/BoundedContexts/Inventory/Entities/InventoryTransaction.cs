using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flowtap_Domain.SharedKernel.Enums;

namespace Flowtap_Domain.BoundedContexts.Inventory.Entities;

public class InventoryTransaction
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid InventoryItemId { get; set; }

    public InventoryItem? InventoryItem { get; set; }

    public InventoryTransactionType Type { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitCost { get; set; }

    [MaxLength(200)]
    public string? ReferenceNumber { get; set; } // PO#, Ticket#, Transfer#

    [MaxLength(50)]
    public string? ReferenceType { get; set; } // PurchaseOrder, Ticket, Transfer

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Creates a new inventory transaction
    /// </summary>
    public static InventoryTransaction Create(
        Guid inventoryItemId,
        InventoryTransactionType type,
        int quantity,
        decimal unitCost,
        string? referenceNumber = null,
        string? referenceType = null)
    {
        if (quantity == 0)
            throw new ArgumentException("Quantity cannot be zero", nameof(quantity));
        if (unitCost < 0)
            throw new ArgumentException("Unit cost cannot be negative", nameof(unitCost));

        return new InventoryTransaction
        {
            Id = Guid.NewGuid(),
            InventoryItemId = inventoryItemId,
            Type = type,
            Quantity = quantity,
            UnitCost = unitCost,
            ReferenceNumber = referenceNumber,
            ReferenceType = referenceType,
            CreatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Calculates the total cost of the transaction
    /// </summary>
    public decimal GetTotalCost()
    {
        return Quantity * UnitCost;
    }

    /// <summary>
    /// Checks if the transaction increases inventory
    /// </summary>
    public bool IsIncrease()
    {
        return Type == InventoryTransactionType.Purchase ||
               Type == InventoryTransactionType.Return ||
               Type == InventoryTransactionType.Found ||
               Type == InventoryTransactionType.Adjustment && Quantity > 0;
    }

    /// <summary>
    /// Checks if the transaction decreases inventory
    /// </summary>
    public bool IsDecrease()
    {
        return Type == InventoryTransactionType.Sale ||
               Type == InventoryTransactionType.Damage ||
               Type == InventoryTransactionType.Loss ||
               Type == InventoryTransactionType.Transfer ||
               Type == InventoryTransactionType.Adjustment && Quantity < 0;
    }
}

