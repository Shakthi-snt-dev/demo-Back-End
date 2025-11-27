using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class ServicePriceMatrix
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? BrandId { get; set; }
    // Note: DeviceBrand is in Service context - reference by ID only, no navigation property

    public Guid? ModelId { get; set; }
    // Note: DeviceModel is in Service context - reference by ID only, no navigation property

    public Guid? ServiceId { get; set; }
    // Note: Service is in Service context - reference by ID only, no navigation property

    [Column(TypeName = "decimal(18,2)")]
    public decimal? CustomPrice { get; set; }

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the custom price
    /// </summary>
    public void UpdateCustomPrice(decimal? customPrice)
    {
        if (customPrice.HasValue && customPrice.Value < 0)
            throw new ArgumentException("Custom price cannot be negative", nameof(customPrice));

        CustomPrice = customPrice;
    }
}

