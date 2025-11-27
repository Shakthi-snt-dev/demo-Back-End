using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class DeviceVariant
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ModelId { get; set; }

    public DeviceModel? Model { get; set; }

    [Required, MaxLength(100)]
    public string Attribute { get; set; } = string.Empty; // Storage, Color, Edition

    [Required, MaxLength(100)]
    public string Value { get; set; } = string.Empty; // 128GB, Black, Pro

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the variant attributes
    /// </summary>
    public void UpdateVariant(string attribute, string value)
    {
        if (string.IsNullOrWhiteSpace(attribute))
            throw new ArgumentException("Attribute cannot be empty", nameof(attribute));
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be empty", nameof(value));
        if (attribute.Length > 100)
            throw new ArgumentException("Attribute cannot exceed 100 characters", nameof(attribute));
        if (value.Length > 100)
            throw new ArgumentException("Value cannot exceed 100 characters", nameof(value));

        Attribute = attribute;
        Value = value;
    }
}

