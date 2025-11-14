using Flowtap_Domain.BoundedContexts.Service.Entities;

namespace Flowtap_Domain.BoundedContexts.Service.Interfaces;

public interface IRepairTicketRepository
{
    Task<RepairTicket?> GetByIdAsync(Guid id);
    Task<IEnumerable<RepairTicket>> GetAllAsync();
    Task<RepairTicket?> GetByTicketNumberAsync(string ticketNumber);
    Task<IEnumerable<RepairTicket>> GetByStatusAsync(string status);
    Task<IEnumerable<RepairTicket>> GetByPriorityAsync(string priority);
    Task<IEnumerable<RepairTicket>> GetByEmployeeIdAsync(Guid employeeId);
    Task<IEnumerable<RepairTicket>> GetOverdueTicketsAsync();
    Task<IEnumerable<RepairTicket>> SearchAsync(string searchTerm);
    Task<RepairTicket> CreateAsync(RepairTicket ticket);
    Task<RepairTicket> UpdateAsync(RepairTicket ticket);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<string> GenerateTicketNumberAsync();
}

