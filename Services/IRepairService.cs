using FlowTap.Api.Models;

namespace FlowTap.Api.Services;

public interface IRepairService
{
    Task<RepairTicket> CreateTicketAsync(CreateRepairTicketRequest request);
    Task<List<RepairTicketDto>> GetTicketsAsync(RepairStatus? status);
    Task<RepairTicketDto> GetTicketByIdAsync(Guid id);
    Task<DiagnosticReport> CreateDiagnosticReportAsync(CreateDiagnosticReportRequest request);
}

public class CreateRepairTicketRequest
{
    public Guid StoreId { get; set; }
    public Guid? CustomerId { get; set; }
    public string Device { get; set; } = string.Empty;
    public string Issue { get; set; } = string.Empty;
    public RepairPriority Priority { get; set; } = RepairPriority.Medium;
    public Guid? TechnicianId { get; set; }
    public decimal? Deposit { get; set; }
}

public class CreateDiagnosticReportRequest
{
    public Guid TicketId { get; set; }
    public string? IMEI { get; set; }
    public string? Model { get; set; }
    public string? OS { get; set; }
    public Dictionary<string, object> Results { get; set; } = new();
}

public class RepairTicketDto
{
    public Guid Id { get; set; }
    public Guid StoreId { get; set; }
    public string? CustomerName { get; set; }
    public string Device { get; set; } = string.Empty;
    public string Issue { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string? TechnicianName { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal? Deposit { get; set; }
    public DateTime CreatedAt { get; set; }
    public DiagnosticReportDto? DiagnosticReport { get; set; }
}

public class DiagnosticReportDto
{
    public Guid Id { get; set; }
    public string? IMEI { get; set; }
    public string? Model { get; set; }
    public string? OS { get; set; }
    public Dictionary<string, object> Results { get; set; } = new();
    public int PassCount { get; set; }
    public int FailCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

