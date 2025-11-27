using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class ServiceWarranty
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ServiceId { get; set; }

    public Service? Service { get; set; }

    public int WarrantyDays { get; set; } = 30;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the warranty period
    /// </summary>
    public void UpdateWarrantyDays(int warrantyDays)
    {
        if (warrantyDays < 0)
            throw new ArgumentException("Warranty days cannot be negative", nameof(warrantyDays));

        WarrantyDays = warrantyDays;
    }

    /// <summary>
    /// Gets the warranty expiration date from a given start date
    /// </summary>
    public DateTime GetWarrantyExpirationDate(DateTime startDate)
    {
        return startDate.AddDays(WarrantyDays);
    }
}

