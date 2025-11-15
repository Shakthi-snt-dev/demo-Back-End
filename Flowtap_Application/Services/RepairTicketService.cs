using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.BoundedContexts.Service.Interfaces;
using Flowtap_Domain.DtoModel;
using Flowtap_Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class RepairTicketService : IRepairTicketService
{
    private readonly IRepairTicketRepository _ticketRepository;
    private readonly ILogger<RepairTicketService> _logger;

    public RepairTicketService(
        IRepairTicketRepository ticketRepository,
        ILogger<RepairTicketService> logger)
    {
        _ticketRepository = ticketRepository;
        _logger = logger;
    }

    public async Task<RepairTicketResponseDto> CreateTicketAsync(CreateRepairTicketRequestDto request)
    {
        var ticket = new RepairTicket
        {
            Id = Guid.NewGuid(),
            StoreId = request.StoreId,
            CustomerId = request.CustomerId,
            CustomerName = request.CustomerName,
            CustomerPhone = request.CustomerPhone,
            CustomerEmail = request.CustomerEmail,
            Device = request.Device,
            Issue = request.Issue,
            Priority = request.Priority.ToLower(),
            EstimatedCost = request.EstimatedCost,
            DepositPaid = request.DepositPaid,
            DueDate = request.DueDate,
            AssignedToEmployeeId = request.AssignedToEmployeeId,
            Notes = request.Notes,
            Status = "pending",
            CreatedDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _ticketRepository.CreateAsync(ticket);
        return MapToDto(created);
    }

    public async Task<RepairTicketResponseDto> GetTicketByIdAsync(Guid id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null)
            throw new EntityNotFoundException("RepairTicket", id);

        return MapToDto(ticket);
    }

    public async Task<RepairTicketResponseDto?> GetTicketByNumberAsync(string ticketNumber)
    {
        var ticket = await _ticketRepository.GetByTicketNumberAsync(ticketNumber);
        return ticket != null ? MapToDto(ticket) : null;
    }

    public async Task<IEnumerable<RepairTicketResponseDto>> GetAllTicketsAsync()
    {
        var tickets = await _ticketRepository.GetAllAsync();
        return tickets.Select(MapToDto);
    }

    public async Task<IEnumerable<RepairTicketResponseDto>> GetTicketsByStatusAsync(string status)
    {
        var tickets = await _ticketRepository.GetByStatusAsync(status);
        return tickets.Select(MapToDto);
    }

    public async Task<IEnumerable<RepairTicketResponseDto>> GetTicketsByPriorityAsync(string priority)
    {
        var tickets = await _ticketRepository.GetByPriorityAsync(priority);
        return tickets.Select(MapToDto);
    }

    public async Task<IEnumerable<RepairTicketResponseDto>> GetTicketsByEmployeeIdAsync(Guid employeeId)
    {
        var tickets = await _ticketRepository.GetByEmployeeIdAsync(employeeId);
        return tickets.Select(MapToDto);
    }

    public async Task<IEnumerable<RepairTicketResponseDto>> GetOverdueTicketsAsync()
    {
        var tickets = await _ticketRepository.GetOverdueTicketsAsync();
        return tickets.Select(MapToDto);
    }

    public async Task<IEnumerable<RepairTicketResponseDto>> SearchTicketsAsync(string searchTerm)
    {
        var tickets = await _ticketRepository.SearchAsync(searchTerm);
        return tickets.Select(MapToDto);
    }

    public async Task<RepairTicketResponseDto> UpdateTicketAsync(Guid id, UpdateRepairTicketRequestDto request)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null)
            throw new EntityNotFoundException("RepairTicket", id);

        if (!string.IsNullOrWhiteSpace(request.CustomerName))
            ticket.CustomerName = request.CustomerName;

        if (request.CustomerPhone != null)
            ticket.CustomerPhone = request.CustomerPhone;

        if (request.CustomerEmail != null)
            ticket.CustomerEmail = request.CustomerEmail;

        if (!string.IsNullOrWhiteSpace(request.Device))
            ticket.Device = request.Device;

        if (!string.IsNullOrWhiteSpace(request.Issue))
            ticket.Issue = request.Issue;

        if (!string.IsNullOrWhiteSpace(request.Status))
            ticket.UpdateStatus(request.Status);

        if (!string.IsNullOrWhiteSpace(request.Priority))
            ticket.UpdatePriority(request.Priority);

        if (request.EstimatedCost.HasValue)
            ticket.UpdateEstimatedCost(request.EstimatedCost.Value);

        if (request.DepositPaid.HasValue)
            ticket.DepositPaid = request.DepositPaid.Value;

        if (request.DueDate.HasValue)
            ticket.UpdateDueDate(request.DueDate.Value);

        if (request.AssignedToEmployeeId.HasValue)
            ticket.AssignToEmployee(request.AssignedToEmployeeId.Value);
        else if (request.AssignedToEmployeeId == Guid.Empty)
            ticket.UnassignEmployee();

        if (!string.IsNullOrWhiteSpace(request.Notes))
            ticket.AddNote(request.Notes);

        var updated = await _ticketRepository.UpdateAsync(ticket);
        return MapToDto(updated);
    }

    public async Task<RepairTicketResponseDto> UpdateTicketStatusAsync(Guid id, string status)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null)
            throw new EntityNotFoundException("RepairTicket", id);

        ticket.UpdateStatus(status);
        var updated = await _ticketRepository.UpdateAsync(ticket);
        return MapToDto(updated);
    }

    public async Task<RepairTicketResponseDto> AssignTicketAsync(Guid id, Guid employeeId)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null)
            throw new EntityNotFoundException("RepairTicket", id);

        ticket.AssignToEmployee(employeeId);
        var updated = await _ticketRepository.UpdateAsync(ticket);
        return MapToDto(updated);
    }

    public async Task<bool> DeleteTicketAsync(Guid id)
    {
        return await _ticketRepository.DeleteAsync(id);
    }

    private static RepairTicketResponseDto MapToDto(RepairTicket ticket)
    {
        return new RepairTicketResponseDto
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            StoreId = ticket.StoreId,
            CustomerId = ticket.CustomerId,
            CustomerName = ticket.CustomerName,
            CustomerPhone = ticket.CustomerPhone,
            CustomerEmail = ticket.CustomerEmail,
            Device = ticket.Device,
            Issue = ticket.Issue,
            Status = ticket.Status,
            Priority = ticket.Priority,
            EstimatedCost = ticket.EstimatedCost,
            DepositPaid = ticket.DepositPaid,
            BalanceDue = ticket.GetBalanceDue(),
            CreatedDate = ticket.CreatedDate,
            DueDate = ticket.DueDate,
            AssignedToEmployeeId = ticket.AssignedToEmployeeId,
            Notes = ticket.Notes,
            IsOverdue = ticket.IsOverdue(),
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt
        };
    }
}

