using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.BoundedContexts.Service.Interfaces;
using Flowtap_Domain.SharedKernel.Enums;
using Flowtap_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flowtap_Infrastructure.Repositories;

public class RepairTicketRepository : IRepairTicketRepository
{
    private readonly AppDbContext _context;

    public RepairTicketRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RepairTicket?> GetByIdAsync(Guid id)
    {
        return await _context.RepairTickets.FindAsync(id);
    }

    public async Task<IEnumerable<RepairTicket>> GetAllAsync()
    {
        return await _context.RepairTickets
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<RepairTicket?> GetByTicketNumberAsync(string ticketNumber)
    {
        return await _context.RepairTickets
            .FirstOrDefaultAsync(t => t.TicketNumber == ticketNumber);
    }

    public async Task<IEnumerable<RepairTicket>> GetByStatusAsync(string status)
    {
        // Convert string to enum for backward compatibility
        if (Enum.TryParse<TicketStatus>(status, true, out var ticketStatus))
        {
            return await _context.RepairTickets
                .Where(t => t.Status == ticketStatus)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
        return new List<RepairTicket>();
    }

    public async Task<IEnumerable<RepairTicket>> GetByPriorityAsync(string priority)
    {
        return await _context.RepairTickets
            .Where(t => t.Priority != null && t.Priority == priority)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<RepairTicket>> GetByEmployeeIdAsync(Guid employeeId)
    {
        return await _context.RepairTickets
            .Where(t => t.TechnicianId == employeeId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<RepairTicket>> GetOverdueTicketsAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.RepairTickets
            .Where(t => (t.EstimatedCompletionAt.HasValue && t.EstimatedCompletionAt.Value < now) ||
                       (t.DueDate.HasValue && t.DueDate.Value < now) &&
                       t.Status != TicketStatus.Completed &&
                       t.Status != TicketStatus.Cancelled)
            .OrderBy(t => t.EstimatedCompletionAt ?? t.DueDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<RepairTicket>> SearchAsync(string searchTerm)
    {
        return await _context.RepairTickets
            .Where(t => (t.TicketNumber != null && t.TicketNumber.Contains(searchTerm)) ||
                       (t.CustomerName != null && t.CustomerName.Contains(searchTerm)) ||
                       (t.DeviceDescription != null && t.DeviceDescription.Contains(searchTerm)) ||
                       (t.ProblemDescription != null && t.ProblemDescription.Contains(searchTerm)))
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<RepairTicket> CreateAsync(RepairTicket ticket)
    {
        ticket.Id = Guid.NewGuid();
        ticket.CreatedAt = DateTime.UtcNow;
        ticket.UpdatedAt = DateTime.UtcNow;
        if (string.IsNullOrWhiteSpace(ticket.TicketNumber))
        {
            ticket.TicketNumber = await GenerateTicketNumberAsync();
        }
        _context.RepairTickets.Add(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }

    public async Task<RepairTicket> UpdateAsync(RepairTicket ticket)
    {
        ticket.UpdatedAt = DateTime.UtcNow;
        _context.RepairTickets.Update(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var ticket = await _context.RepairTickets.FindAsync(id);
        if (ticket == null)
            return false;

        _context.RepairTickets.Remove(ticket);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.RepairTickets.AnyAsync(t => t.Id == id);
    }

    public async Task<string> GenerateTicketNumberAsync()
    {
        var lastTicket = await _context.RepairTickets
            .OrderByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync();

        int nextNumber = 1;
        if (lastTicket != null && !string.IsNullOrWhiteSpace(lastTicket.TicketNumber))
        {
            var numberPart = lastTicket.TicketNumber.Replace("TKT-", "").Replace("RD-", "");
            if (int.TryParse(numberPart, out int lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }

        var year = DateTime.UtcNow.Year;
        return $"RD-{year}-{nextNumber:D4}";
    }
}

