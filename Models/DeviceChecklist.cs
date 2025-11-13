namespace FlowTap.Api.Models;

public class DeviceChecklist
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TicketId { get; set; }
    public RepairTicket Ticket { get; set; } = null!;
    public Dictionary<string, object> ConditionChecks { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

