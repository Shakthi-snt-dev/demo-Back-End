using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class ServiceDiagnosis
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid TicketId { get; set; }

    public RepairTicket? Ticket { get; set; }

    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;

    public bool RequiresAdvancedRepair { get; set; }

    public Guid? RecommendedServiceId { get; set; }
    // Note: Service is in Service context - reference by ID only, no navigation property

    public DateTime LoggedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the diagnosis notes
    /// </summary>
    public void UpdateNotes(string notes)
    {
        if (notes != null && notes.Length > 1000)
            throw new ArgumentException("Notes cannot exceed 1000 characters", nameof(notes));

        Notes = notes ?? string.Empty;
    }

    /// <summary>
    /// Sets whether advanced repair is required
    /// </summary>
    public void SetRequiresAdvancedRepair(bool requiresAdvancedRepair)
    {
        RequiresAdvancedRepair = requiresAdvancedRepair;
    }

    /// <summary>
    /// Sets the recommended service
    /// </summary>
    public void SetRecommendedService(Guid? serviceId)
    {
        RecommendedServiceId = serviceId;
    }
}

