using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.BoundedContexts.Service.Interfaces;
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
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }

    public async Task<RepairTicket?> GetByTicketNumberAsync(string ticketNumber)
    {
        return await _context.RepairTickets
            .FirstOrDefaultAsync(t => t.TicketNumber == ticketNumber);
    }

    public async Task<IEnumerable<RepairTicket>> GetByStatusAsync(string status)
    {
        return await _context.RepairTickets
            .Where(t => t.Status == status)
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<RepairTicket>> GetByPriorityAsync(string priority)
    {
        return await _context.RepairTickets
            .Where(t => t.Priority == priority)
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<RepairTicket>> GetByEmployeeIdAsync(Guid employeeId)
    {
        return await _context.RepairTickets
            .Where(t => t.AssignedToEmployeeId == employeeId)
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<RepairTicket>> GetOverdueTicketsAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.RepairTickets
            .Where(t => t.DueDate.HasValue &&
                       t.DueDate.Value < now &&
                       t.Status != "completed" &&
                       t.Status != "cancelled")
            .OrderBy(t => t.DueDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<RepairTicket>> SearchAsync(string searchTerm)
    {
        return await _context.RepairTickets
            .Where(t => t.TicketNumber.Contains(searchTerm) ||
                       t.CustomerName.Contains(searchTerm) ||
                       t.Device.Contains(searchTerm) ||
                       t.Issue.Contains(searchTerm))
            .OrderByDescending(t => t.CreatedDate)
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
            .OrderByDescending(t => t.CreatedDate)
            .FirstOrDefaultAsync();

        int nextNumber = 1;
        if (lastTicket != null && !string.IsNullOrWhiteSpace(lastTicket.TicketNumber))
        {
            var numberPart = lastTicket.TicketNumber.Replace("TKT-", "");
            if (int.TryParse(numberPart, out int lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }

        return $"TKT-{nextNumber:D3}";
    }
}

