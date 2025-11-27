using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class Service
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid StoreId { get; set; }
    // Note: Store is in Store context - reference by ID only, no navigation property

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal BasePrice { get; set; }

    public bool IsDeviceSpecific { get; set; } = false;

    public bool IsActive { get; set; } = true;

    [MaxLength(50)]
    public string? TaxClass { get; set; } // Tax classification

    public bool ShowOnPOS { get; set; } = true; // Show on POS toggle

    public bool ShowOnWidget { get; set; } = true; // Show on Widget toggle

    public ICollection<ServicePart> Parts { get; set; } = new List<ServicePart>();

    public ICollection<ServiceLabor> Labor { get; set; } = new List<ServiceLabor>();

    public ICollection<ServiceWarranty> Warranties { get; set; } = new List<ServiceWarranty>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the service information
    /// </summary>
    public void UpdateServiceInfo(string name, string? description, decimal basePrice, bool isDeviceSpecific, string? taxClass = null, bool? showOnPOS = null, bool? showOnWidget = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (basePrice < 0)
            throw new ArgumentException("Base price cannot be negative", nameof(basePrice));

        Name = name;
        Description = description;
        BasePrice = basePrice;
        IsDeviceSpecific = isDeviceSpecific;
        
        if (taxClass != null)
            TaxClass = taxClass;
        
        if (showOnPOS.HasValue)
            ShowOnPOS = showOnPOS.Value;
        
        if (showOnWidget.HasValue)
            ShowOnWidget = showOnWidget.Value;

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the base price
    /// </summary>
    public void UpdateBasePrice(decimal basePrice)
    {
        if (basePrice < 0)
            throw new ArgumentException("Base price cannot be negative", nameof(basePrice));

        BasePrice = basePrice;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Activates the service
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivates the service
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculates total service cost (base price + labor)
    /// Note: Parts cost should be calculated separately as Product prices may vary
    /// </summary>
    public decimal GetTotalCost()
    {
        var laborCost = Labor?.Sum(l => l.Cost) ?? 0;
        return BasePrice + laborCost;
    }
}

