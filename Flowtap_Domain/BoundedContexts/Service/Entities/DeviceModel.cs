using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class DeviceModel
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid BrandId { get; set; }

    public DeviceBrand? Brand { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public ICollection<DeviceVariant> Variants { get; set; } = new List<DeviceVariant>();

    public ICollection<DeviceProblem> Problems { get; set; } = new List<DeviceProblem>();

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the model name
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
    /// Changes the parent brand
    /// </summary>
    public void ChangeBrand(Guid brandId)
    {
        BrandId = brandId;
    }
}

