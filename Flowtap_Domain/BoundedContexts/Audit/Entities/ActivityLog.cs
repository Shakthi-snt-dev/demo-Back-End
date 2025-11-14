using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Audit.Entities;

public class ActivityLog
{
    [Key]
    public Guid Id { get; set; }

    public Guid? AppUserId { get; set; }

    public Guid? StoreId { get; set; }

    public Guid? EmployeeId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string? Data { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

