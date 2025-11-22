using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class PartUsed
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid RepairTicketId { get; set; }

    public RepairTicket? RepairTicket { get; set; }

    [Required]
    public Guid InventoryItemId { get; set; } // Reference to Inventory context (ID only, no navigation)

    public int Quantity { get; set; } = 1;

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; } = 0m;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice => UnitPrice * Quantity;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the quantity used
    /// </summary>
    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidOperationException("Quantity must be greater than zero");

        Quantity = quantity;
    }

    /// <summary>
    /// Updates the unit price
    /// </summary>
    public void UpdateUnitPrice(decimal unitPrice)
    {
        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));

        UnitPrice = unitPrice;
    }

    /// <summary>
    /// Updates both quantity and unit price
    /// </summary>
    public void UpdatePartUsage(int quantity, decimal unitPrice)
    {
        UpdateQuantity(quantity);
        UpdateUnitPrice(unitPrice);
    }
}

