using System.ComponentModel.DataAnnotations;
using Flowtap_Domain.SharedKernel.Enums;

namespace Flowtap_Application.DtoModel.Request;

public class CreateProductRequestDto
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? SKU { get; set; }

    [MaxLength(100)]
    public string? Brand { get; set; }

    public string? Description { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? SubCategoryId { get; set; }

    [Range(0, double.MaxValue)]
    public decimal CostPrice { get; set; }

    [Range(0, double.MaxValue)]
    public decimal SalePrice { get; set; }

    public ProductType ProductType { get; set; } = ProductType.InventoryPart;

    public bool TrackSerials { get; set; } = false;

    public Guid? SupplierId { get; set; }

    // Additional fields for Inventory Part
    [MaxLength(100)]
    public string? Condition { get; set; }

    [MaxLength(50)]
    public string? InventoryValuationMethod { get; set; }

    [Range(0, double.MaxValue)]
    public decimal MinimumPrice { get; set; } = 0;

    [MaxLength(50)]
    public string? TaxClass { get; set; }

    public bool ShowOnPOS { get; set; } = true;

    // Stock Controls
    public int? OnHandQty { get; set; }

    public int? StockWarning { get; set; }

    public int? ReorderLevel { get; set; }

    // Warranty
    public int? WarrantyDays { get; set; }

    // UPC Code
    [MaxLength(100)]
    public string? UPCCode { get; set; }
}

public class UpdateProductRequestDto
{
    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(100)]
    public string? SKU { get; set; }

    [MaxLength(100)]
    public string? Brand { get; set; }

    public string? Description { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? SubCategoryId { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? CostPrice { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? SalePrice { get; set; }

    public ProductType? ProductType { get; set; }

    public bool? TrackSerials { get; set; }

    public bool? IsActive { get; set; }

    [MaxLength(100)]
    public string? Condition { get; set; }

    [MaxLength(50)]
    public string? InventoryValuationMethod { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? MinimumPrice { get; set; }

    [MaxLength(50)]
    public string? TaxClass { get; set; }

    public bool? ShowOnPOS { get; set; }

    public Guid? SupplierId { get; set; }
}

public class CreateSpecialOrderPartRequestDto
{
    [Required]
    public Guid StoreId { get; set; }

    [Required, MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    public Guid? ManufacturerId { get; set; }

    public Guid? DeviceModelId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int RequiredQty { get; set; } = 1;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal UnitCost { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal RetailPrice { get; set; }

    [Required]
    public Guid SupplierId { get; set; }

    [MaxLength(500)]
    public string? OrderLink { get; set; }

    [MaxLength(200)]
    public string? TrackingId { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? ReceivedDate { get; set; }

    public bool IsTaxExclusive { get; set; } = false;

    public bool CreatePurchaseOrder { get; set; } = false;
}

public class UpdateSpecialOrderPartRequestDto
{
    [MaxLength(200)]
    public string? ItemName { get; set; }

    public Guid? ManufacturerId { get; set; }

    public Guid? DeviceModelId { get; set; }

    [Range(1, int.MaxValue)]
    public int? RequiredQty { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? UnitCost { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? RetailPrice { get; set; }

    public Guid? SupplierId { get; set; }

    [MaxLength(500)]
    public string? OrderLink { get; set; }

    [MaxLength(200)]
    public string? TrackingId { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? ReceivedDate { get; set; }

    public bool? IsTaxExclusive { get; set; }

    public bool? CreatePurchaseOrder { get; set; }
}
