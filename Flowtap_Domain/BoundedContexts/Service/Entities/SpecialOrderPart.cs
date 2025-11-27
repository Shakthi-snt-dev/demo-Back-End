using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class SpecialOrderPart
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid StoreId { get; set; }
    // Note: Store is in Store context - reference by ID only, no navigation property

    [Required, MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    public Guid? ManufacturerId { get; set; } // DeviceBrand
    // Note: DeviceBrand is in Service context - reference by ID only, no navigation property

    public Guid? DeviceModelId { get; set; } // DeviceModel
    // Note: DeviceModel is in Service context - reference by ID only, no navigation property

    [Required]
    [Range(1, int.MaxValue)]
    public int RequiredQty { get; set; } = 1;

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitCost { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal RetailPrice { get; set; }

    [Required]
    public Guid SupplierId { get; set; }
    // Note: Supplier is in Procurement context - reference by ID only, no navigation property

    [MaxLength(500)]
    public string? OrderLink { get; set; }

    [MaxLength(200)]
    public string? TrackingId { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? ReceivedDate { get; set; }

    public bool IsTaxExclusive { get; set; } = false; // Mark as Tax Exclusive

    public bool CreatePurchaseOrder { get; set; } = false; // Create Purchase Order toggle

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the special order part information
    /// </summary>
    public void UpdatePartInfo(string itemName, int requiredQty, decimal unitCost, decimal retailPrice, Guid supplierId)
    {
        if (string.IsNullOrWhiteSpace(itemName))
            throw new ArgumentException("Item name cannot be empty", nameof(itemName));
        if (requiredQty <= 0)
            throw new ArgumentException("Required quantity must be greater than zero", nameof(requiredQty));
        if (unitCost < 0)
            throw new ArgumentException("Unit cost cannot be negative", nameof(unitCost));
        if (retailPrice < 0)
            throw new ArgumentException("Retail price cannot be negative", nameof(retailPrice));

        ItemName = itemName;
        RequiredQty = requiredQty;
        UnitCost = unitCost;
        RetailPrice = retailPrice;
        SupplierId = supplierId;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates order details
    /// </summary>
    public void UpdateOrderDetails(string? orderLink, string? trackingId, string? notes, DateTime? orderDate, DateTime? receivedDate)
    {
        OrderLink = orderLink;
        TrackingId = trackingId;
        Notes = notes;
        OrderDate = orderDate;
        ReceivedDate = receivedDate;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the order as received
    /// </summary>
    public void MarkAsReceived(DateTime? receivedDate = null)
    {
        ReceivedDate = receivedDate ?? DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculates total cost
    /// </summary>
    public decimal GetTotalCost()
    {
        return RequiredQty * UnitCost;
    }
}

