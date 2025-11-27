using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class ServicePart
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ServiceId { get; set; }

    public Service? Service { get; set; }

    [Required]
    public Guid ProductId { get; set; } // Part from Inventory
    // Note: Product is in Inventory context - reference by ID only, no navigation property

    public int Quantity { get; set; } = 1;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

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

