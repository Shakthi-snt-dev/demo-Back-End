using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class DeviceCategory
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? PictureUrl { get; set; } // Cloud storage URL for uploaded picture

    public bool IsLaborBillable { get; set; } = false; // Mark Category as Labor/Billable Hours

    public ICollection<DeviceBrand> Brands { get; set; } = new List<DeviceBrand>();

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the category name
    /// </summary>
    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (name.Length > 150)
            throw new ArgumentException("Name cannot exceed 150 characters", nameof(name));

        Name = name;
    }

    /// <summary>
    /// Updates the picture URL
    /// </summary>
    public void UpdatePictureUrl(string? pictureUrl)
    {
        if (pictureUrl != null && pictureUrl.Length > 500)
            throw new ArgumentException("Picture URL cannot exceed 500 characters", nameof(pictureUrl));

        PictureUrl = pictureUrl;
    }

    /// <summary>
    /// Sets whether the category is labor billable
    /// </summary>
    public void SetLaborBillable(bool isLaborBillable)
    {
        IsLaborBillable = isLaborBillable;
    }
}

