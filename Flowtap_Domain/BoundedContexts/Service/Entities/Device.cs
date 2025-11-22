using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class Device
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid StoreId { get; set; }

    [Required, MaxLength(200)]
    public string Brand { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Model { get; set; }

    [MaxLength(200)]
    public string? SerialNumber { get; set; }

    [MaxLength(200)]
    public string? IMEI { get; set; }

    public ICollection<RepairTicket> RepairTickets { get; set; } = new List<RepairTicket>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the device information
    /// </summary>
    public void UpdateDeviceInfo(string brand, string? model, string? serialNumber, string? imei)
    {
        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentException("Brand cannot be empty", nameof(brand));

        Brand = brand;
        Model = model;
        SerialNumber = serialNumber;
        IMEI = imei;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the serial number
    /// </summary>
    public void UpdateSerialNumber(string serialNumber)
    {
        SerialNumber = serialNumber;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the IMEI
    /// </summary>
    public void UpdateIMEI(string imei)
    {
        IMEI = imei;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the number of repair tickets for this device
    /// </summary>
    public int GetRepairTicketCount()
    {
        return RepairTickets.Count;
    }
}

