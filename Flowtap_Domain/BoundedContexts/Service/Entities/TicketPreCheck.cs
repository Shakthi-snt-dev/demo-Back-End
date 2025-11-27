using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Service.Entities;

public class TicketPreCheck
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid RepairTicketId { get; set; }

    public RepairTicket? RepairTicket { get; set; }

    [Required]
    public Guid PreCheckItemId { get; set; }

    public PreCheckItem? PreCheckItem { get; set; }

    public bool Passed { get; set; }

    public DateTime CheckedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Marks the pre-check as passed
    /// </summary>
    public void MarkAsPassed()
    {
        Passed = true;
        CheckedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the pre-check as failed
    /// </summary>
    public void MarkAsFailed()
    {
        Passed = false;
        CheckedAt = DateTime.UtcNow;
    }
}

