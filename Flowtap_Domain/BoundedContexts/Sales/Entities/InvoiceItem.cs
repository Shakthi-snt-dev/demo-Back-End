using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flowtap_Domain.BoundedContexts.Sales.Entities;

public class InvoiceItem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid InvoiceId { get; set; }

    public Invoice? Invoice { get; set; }

    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    public int Quantity { get; set; } = 1;

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; } = 0m;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total => UnitPrice * Quantity;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the item description
    /// </summary>
    public void UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));

        Description = description;
    }

    /// <summary>
    /// Updates the quantity
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
    /// Updates the item details
    /// </summary>
    public void UpdateItem(string description, int quantity, decimal unitPrice)
    {
        UpdateDescription(description);
        UpdateQuantity(quantity);
        UpdateUnitPrice(unitPrice);
    }
}

