using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Inventory.Entities;

public class InventorySettings
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid StoreId { get; set; }
    // Note: Store is in Store context - reference by ID only, no navigation property

    public bool AllowNegativeInventory { get; set; } = false;

    public bool AutoGenerateSKU { get; set; } = true;

    public int DefaultReorderLevel { get; set; } = 1;

    public int DefaultReorderQuantity { get; set; } = 1;

    public bool TrackSerialNumbersByDefault { get; set; } = false;

    public bool RequireSerialScanOnSale { get; set; } = false;

    public bool EnableLowStockAlerts { get; set; } = true;

    [MaxLength(320)]
    public string? LowStockAlertEmail { get; set; }

    public bool AutoAdjustInventoryOnTicketCompletion { get; set; } = true;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the inventory settings
    /// </summary>
    public void UpdateSettings(
        bool allowNegativeInventory,
        bool autoGenerateSKU,
        int defaultReorderLevel,
        int defaultReorderQuantity,
        bool trackSerialNumbersByDefault,
        bool requireSerialScanOnSale,
        bool enableLowStockAlerts,
        string? lowStockAlertEmail,
        bool autoAdjustInventoryOnTicketCompletion)
    {
        if (defaultReorderLevel < 0)
            throw new ArgumentException("Default reorder level cannot be negative", nameof(defaultReorderLevel));
        if (defaultReorderQuantity < 0)
            throw new ArgumentException("Default reorder quantity cannot be negative", nameof(defaultReorderQuantity));

        AllowNegativeInventory = allowNegativeInventory;
        AutoGenerateSKU = autoGenerateSKU;
        DefaultReorderLevel = defaultReorderLevel;
        DefaultReorderQuantity = defaultReorderQuantity;
        TrackSerialNumbersByDefault = trackSerialNumbersByDefault;
        RequireSerialScanOnSale = requireSerialScanOnSale;
        EnableLowStockAlerts = enableLowStockAlerts;
        LowStockAlertEmail = lowStockAlertEmail;
        AutoAdjustInventoryOnTicketCompletion = autoAdjustInventoryOnTicketCompletion;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Enables or disables negative inventory
    /// </summary>
    public void SetAllowNegativeInventory(bool allow)
    {
        AllowNegativeInventory = allow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the default reorder level
    /// </summary>
    public void UpdateDefaultReorderLevel(int level)
    {
        if (level < 0)
            throw new ArgumentException("Default reorder level cannot be negative", nameof(level));

        DefaultReorderLevel = level;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the default reorder quantity
    /// </summary>
    public void UpdateDefaultReorderQuantity(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Default reorder quantity cannot be negative", nameof(quantity));

        DefaultReorderQuantity = quantity;
        UpdatedAt = DateTime.UtcNow;
    }
}

