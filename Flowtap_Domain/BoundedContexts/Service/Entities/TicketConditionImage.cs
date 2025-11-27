using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class TicketConditionImage
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid RepairTicketId { get; set; }

    public RepairTicket? RepairTicket { get; set; }

    [Required]
    public string ImageUrl { get; set; } = string.Empty; // Cloud URL

    [MaxLength(200)]
    public string? Description { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the image URL
    /// </summary>
    public void UpdateImageUrl(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException("Image URL cannot be empty", nameof(imageUrl));

        ImageUrl = imageUrl;
    }

    /// <summary>
    /// Updates the description
    /// </summary>
    public void UpdateDescription(string? description)
    {
        if (description != null && description.Length > 200)
            throw new ArgumentException("Description cannot exceed 200 characters", nameof(description));

        Description = description;
    }
}

