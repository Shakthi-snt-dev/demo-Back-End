using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flowtap_Domain.SharedKernel.Enums;

namespace Flowtap_Domain.BoundedContexts.Inventory.Entities;

public class Product
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? SKU { get; set; }

    [MaxLength(100)]
    public string? Brand { get; set; }

    public string? Description { get; set; }

    public Guid? CategoryId { get; set; }
    public ProductCategory? Category { get; set; }

    public Guid? SubCategoryId { get; set; }
    public ProductSubCategory? SubCategory { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal CostPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal SalePrice { get; set; }

    public ProductType ProductType { get; set; } = ProductType.InventoryPart;

    public bool TrackSerials { get; set; } = false;

    public bool IsActive { get; set; } = true;

    public Guid? SupplierId { get; set; }

    [MaxLength(100)]
    public string? Condition { get; set; } // New, Used, Refurbished, etc.

    [MaxLength(50)]
    public string? InventoryValuationMethod { get; set; } // FIFO, LIFO, Average Cost

    [Column(TypeName = "decimal(18,2)")]
    public decimal MinimumPrice { get; set; } = 0; // Minimum selling price

    [MaxLength(50)]
    public string? TaxClass { get; set; } // Tax classification

    public bool ShowOnPOS { get; set; } = true; // Show on POS toggle
    
    [MaxLength(100)]
    public string? UPCCode { get; set; } // Universal Product Code / Barcode

    public int? WarrantyDays { get; set; } // Warranty period in days
    
    // Note: Supplier is in Procurement context - reference by ID only, no navigation property

    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the product information
    /// </summary>
    public void UpdateProductInfo(string name, string? sku, string? brand, string? description, decimal costPrice, decimal salePrice, 
        string? condition = null, string? inventoryValuationMethod = null, decimal? minimumPrice = null, 
        string? taxClass = null, bool? showOnPOS = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (costPrice < 0)
            throw new ArgumentException("Cost price cannot be negative", nameof(costPrice));
        if (salePrice < 0)
            throw new ArgumentException("Sale price cannot be negative", nameof(salePrice));
        if (minimumPrice.HasValue && minimumPrice.Value < 0)
            throw new ArgumentException("Minimum price cannot be negative", nameof(minimumPrice));

        Name = name;
        SKU = sku;
        Brand = brand;
        Description = description;
        CostPrice = costPrice;
        SalePrice = salePrice;

        if (condition != null)
            Condition = condition;
        if (inventoryValuationMethod != null)
            InventoryValuationMethod = inventoryValuationMethod;
        if (minimumPrice.HasValue)
            MinimumPrice = minimumPrice.Value;
        if (taxClass != null)
            TaxClass = taxClass;
        if (showOnPOS.HasValue)
            ShowOnPOS = showOnPOS.Value;
    }

    /// <summary>
    /// Updates the cost price
    /// </summary>
    public void UpdateCostPrice(decimal costPrice)
    {
        if (costPrice < 0)
            throw new ArgumentException("Cost price cannot be negative", nameof(costPrice));

        CostPrice = costPrice;
    }

    /// <summary>
    /// Updates the sale price
    /// </summary>
    public void UpdateSalePrice(decimal salePrice)
    {
        if (salePrice < 0)
            throw new ArgumentException("Sale price cannot be negative", nameof(salePrice));

        SalePrice = salePrice;
    }

    /// <summary>
    /// Sets the category for the product
    /// </summary>
    public void SetCategory(Guid? categoryId, Guid? subCategoryId)
    {
        CategoryId = categoryId;
        SubCategoryId = subCategoryId;
    }

    /// <summary>
    /// Sets the supplier for the product
    /// </summary>
    public void SetSupplier(Guid? supplierId)
    {
        SupplierId = supplierId;
    }

    /// <summary>
    /// Enables or disables serial number tracking
    /// </summary>
    public void SetTrackSerials(bool trackSerials)
    {
        TrackSerials = trackSerials;
    }

    /// <summary>
    /// Sets the product type (InventoryPart, ServicePart, or Service)
    /// </summary>
    public void SetProductType(ProductType productType)
    {
        ProductType = productType;
    }

    /// <summary>
    /// Calculates the profit margin percentage
    /// </summary>
    public decimal GetProfitMargin()
    {
        if (SalePrice == 0)
            return 0;

        return ((SalePrice - CostPrice) / SalePrice) * 100;
    }

    /// <summary>
    /// Calculates the profit amount
    /// </summary>
    public decimal GetProfitAmount()
    {
        return SalePrice - CostPrice;
    }
}

