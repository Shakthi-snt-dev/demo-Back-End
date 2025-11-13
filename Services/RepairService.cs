using FlowTap.Api.Data;
using FlowTap.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowTap.Api.Services;

public class RepairService : IRepairService
{
    private readonly ApplicationDbContext _context;

    public RepairService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RepairTicket> CreateTicketAsync(CreateRepairTicketRequest request)
    {
        var ticket = new RepairTicket
        {
            StoreId = request.StoreId,
            CustomerId = request.CustomerId,
            Device = request.Device,
            Issue = request.Issue,
            Priority = request.Priority,
            TechnicianId = request.TechnicianId,
            Deposit = request.Deposit,
            Status = RepairStatus.New,
            CreatedAt = DateTime.UtcNow
        };

        _context.RepairTickets.Add(ticket);
        await _context.SaveChangesAsync();

        return ticket;
    }

    public async Task<List<RepairTicketDto>> GetTicketsAsync(RepairStatus? status)
    {
        var query = _context.RepairTickets.AsQueryable();

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        var tickets = await query
            .Include(t => t.Customer)
            .Include(t => t.Technician)
            .Include(t => t.DiagnosticReport)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return tickets.Select(t => new RepairTicketDto
        {
            Id = t.Id,
            StoreId = t.StoreId,
            CustomerName = t.Customer?.Name,
            Device = t.Device,
            Issue = t.Issue,
            Priority = t.Priority.ToString(),
            TechnicianName = t.Technician?.Name,
            Status = t.Status.ToString(),
            Deposit = t.Deposit,
            CreatedAt = t.CreatedAt,
            DiagnosticReport = t.DiagnosticReport != null ? new DiagnosticReportDto
            {
                Id = t.DiagnosticReport.Id,
                IMEI = t.DiagnosticReport.IMEI,
                Model = t.DiagnosticReport.Model,
                OS = t.DiagnosticReport.OS,
                Results = t.DiagnosticReport.Results,
                PassCount = t.DiagnosticReport.PassCount,
                FailCount = t.DiagnosticReport.FailCount,
                CreatedAt = t.DiagnosticReport.CreatedAt
            } : null
        }).ToList();
    }

    public async Task<RepairTicketDto> GetTicketByIdAsync(Guid id)
    {
        var ticket = await _context.RepairTickets
            .Include(t => t.Customer)
            .Include(t => t.Technician)
            .Include(t => t.DiagnosticReport)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
            throw new Exception("Repair ticket not found");

        return new RepairTicketDto
        {
            Id = ticket.Id,
            StoreId = ticket.StoreId,
            CustomerName = ticket.Customer?.Name,
            Device = ticket.Device,
            Issue = ticket.Issue,
            Priority = ticket.Priority.ToString(),
            TechnicianName = ticket.Technician?.Name,
            Status = ticket.Status.ToString(),
            Deposit = ticket.Deposit,
            CreatedAt = ticket.CreatedAt,
            DiagnosticReport = ticket.DiagnosticReport != null ? new DiagnosticReportDto
            {
                Id = ticket.DiagnosticReport.Id,
                IMEI = ticket.DiagnosticReport.IMEI,
                Model = ticket.DiagnosticReport.Model,
                OS = ticket.DiagnosticReport.OS,
                Results = ticket.DiagnosticReport.Results,
                PassCount = ticket.DiagnosticReport.PassCount,
                FailCount = ticket.DiagnosticReport.FailCount,
                CreatedAt = ticket.DiagnosticReport.CreatedAt
            } : null
        };
    }

    public async Task<DiagnosticReport> CreateDiagnosticReportAsync(CreateDiagnosticReportRequest request)
    {
        var ticket = await _context.RepairTickets.FindAsync(request.TicketId);
        if (ticket == null)
            throw new Exception("Repair ticket not found");

        var report = new DiagnosticReport
        {
            TicketId = request.TicketId,
            IMEI = request.IMEI,
            Model = request.Model,
            OS = request.OS,
            Results = request.Results,
            CreatedAt = DateTime.UtcNow
        };

        // Calculate pass/fail counts
        report.PassCount = request.Results.Values.Count(v => v.ToString()?.ToLower().Contains("pass") == true);
        report.FailCount = request.Results.Values.Count(v => v.ToString()?.ToLower().Contains("fail") == true);

        _context.DiagnosticReports.Add(report);
        ticket.Status = RepairStatus.Diagnosing;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return report;
    }
}

