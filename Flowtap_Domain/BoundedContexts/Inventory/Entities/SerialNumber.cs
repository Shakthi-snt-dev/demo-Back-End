using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Inventory.Entities;

public class SerialNumber
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid InventoryItemId { get; set; }

    public InventoryItem? InventoryItem { get; set; }

    [Required, MaxLength(200)]
    public string Serial { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Status { get; set; } = "Available";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the serial number
    /// </summary>
    public void UpdateSerial(string serial)
    {
        if (string.IsNullOrWhiteSpace(serial))
            throw new ArgumentException("Serial number cannot be empty", nameof(serial));
        if (serial.Length > 200)
            throw new ArgumentException("Serial number cannot exceed 200 characters", nameof(serial));

        Serial = serial;
    }

    /// <summary>
    /// Updates the status of the serial number
    /// </summary>
    public void UpdateStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status cannot be empty", nameof(status));
        if (status.Length > 50)
            throw new ArgumentException("Status cannot exceed 50 characters", nameof(status));

        Status = status;
    }

    /// <summary>
    /// Marks the serial number as available
    /// </summary>
    public void MarkAsAvailable()
    {
        Status = "Available";
    }

    /// <summary>
    /// Marks the serial number as sold
    /// </summary>
    public void MarkAsSold()
    {
        Status = "Sold";
    }

    /// <summary>
    /// Marks the serial number as damaged
    /// </summary>
    public void MarkAsDamaged()
    {
        Status = "Damaged";
    }

    /// <summary>
    /// Checks if the serial number is available
    /// </summary>
    public bool IsAvailable()
    {
        return Status == "Available";
    }
}

