namespace FlowTap.Api.Models;

public enum RepairPriority
{
    Low,
    Medium,
    High,
    Urgent
}

public enum RepairStatus
{
    New,
    Diagnosing,
    AwaitingParts,
    InProgress,
    Repaired,
    Delivered,
    Cancelled
}

public class RepairTicket
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StoreId { get; set; }
    public Store Store { get; set; } = null!;
    public Guid? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public string Device { get; set; } = string.Empty;
    public string Issue { get; set; } = string.Empty;
    public RepairPriority Priority { get; set; } = RepairPriority.Medium;
    public Guid? TechnicianId { get; set; }
    public Employee? Technician { get; set; }
    public RepairStatus Status { get; set; } = RepairStatus.New;
    public decimal? Deposit { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public DiagnosticReport? DiagnosticReport { get; set; }
    public DeviceChecklist? DeviceChecklist { get; set; }
}

