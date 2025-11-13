namespace FlowTap.Api.Models;

public class DiagnosticReport
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TicketId { get; set; }
    public RepairTicket Ticket { get; set; } = null!;
    public string? IMEI { get; set; }
    public string? Model { get; set; }
    public string? OS { get; set; }
    public Dictionary<string, object> Results { get; set; } = new();
    public int PassCount { get; set; }
    public int FailCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

