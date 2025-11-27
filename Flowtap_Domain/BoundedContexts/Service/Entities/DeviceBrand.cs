using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class DeviceBrand
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid CategoryId { get; set; }

    public DeviceCategory? Category { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public ICollection<DeviceModel> Models { get; set; } = new List<DeviceModel>();

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the brand name
    /// </summary>
    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (name.Length > 200)
            throw new ArgumentException("Name cannot exceed 200 characters", nameof(name));

        Name = name;
    }

    /// <summary>
    /// Changes the parent category
    /// </summary>
    public void ChangeCategory(Guid categoryId)
    {
        CategoryId = categoryId;
    }
}

