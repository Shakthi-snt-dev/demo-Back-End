using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class DeviceProblem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ModelId { get; set; }

    public DeviceModel? Model { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    // Maps problem → service directly
    public Guid? ServiceId { get; set; }
    // Note: Service is in Service context - reference by ID only, no navigation property

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the problem information
    /// </summary>
    public void UpdateProblemInfo(string title, string? description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));
        if (title.Length > 200)
            throw new ArgumentException("Title cannot exceed 200 characters", nameof(title));

        Title = title;
        Description = description;
    }

    /// <summary>
    /// Links the problem to a service
    /// </summary>
    public void LinkService(Guid? serviceId)
    {
        ServiceId = serviceId;
    }
}

