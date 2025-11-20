using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;

namespace Flowtap_Application.Interfaces;

public interface IRepairTicketService
{
    Task<RepairTicketResponseDto> CreateTicketAsync(CreateRepairTicketRequestDto request);
    Task<RepairTicketResponseDto> GetTicketByIdAsync(Guid id);
    Task<RepairTicketResponseDto?> GetTicketByNumberAsync(string ticketNumber);
    Task<IEnumerable<RepairTicketResponseDto>> GetAllTicketsAsync();
    Task<IEnumerable<RepairTicketResponseDto>> GetTicketsByStatusAsync(string status);
    Task<IEnumerable<RepairTicketResponseDto>> GetTicketsByPriorityAsync(string priority);
    Task<IEnumerable<RepairTicketResponseDto>> GetTicketsByEmployeeIdAsync(Guid employeeId);
    Task<IEnumerable<RepairTicketResponseDto>> GetOverdueTicketsAsync();
    Task<IEnumerable<RepairTicketResponseDto>> SearchTicketsAsync(string searchTerm);
    Task<RepairTicketResponseDto> UpdateTicketAsync(Guid id, UpdateRepairTicketRequestDto request);
    Task<RepairTicketResponseDto> UpdateTicketStatusAsync(Guid id, string status);
    Task<RepairTicketResponseDto> AssignTicketAsync(Guid id, Guid employeeId);
    Task<bool> DeleteTicketAsync(Guid id);
}

