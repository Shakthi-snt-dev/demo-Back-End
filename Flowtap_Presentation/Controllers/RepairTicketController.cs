using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RepairTicketController : ControllerBase
{
    private readonly IRepairTicketService _ticketService;
    private readonly ILogger<RepairTicketController> _logger;

    public RepairTicketController(
        IRepairTicketService ticketService,
        ILogger<RepairTicketController> logger)
    {
        _ticketService = ticketService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new repair ticket
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<RepairTicketResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<RepairTicketResponseDto>>> CreateTicket([FromBody] CreateRepairTicketRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<RepairTicketResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _ticketService.CreateTicketAsync(request);
        return Ok(ApiResponseDto<RepairTicketResponseDto>.Success(result, "Ticket created successfully"));
    }

    /// <summary>
    /// Get ticket by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<RepairTicketResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<RepairTicketResponseDto>>> GetTicket(Guid id)
    {
        var result = await _ticketService.GetTicketByIdAsync(id);
        return Ok(ApiResponseDto<RepairTicketResponseDto>.Success(result, "Ticket retrieved successfully"));
    }

    /// <summary>
    /// Get ticket by ticket number
    /// </summary>
    [HttpGet("number/{ticketNumber}")]
    [ProducesResponseType(typeof(ApiResponseDto<RepairTicketResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<RepairTicketResponseDto>>> GetTicketByNumber(string ticketNumber)
    {
        var result = await _ticketService.GetTicketByNumberAsync(ticketNumber);
        if (result == null)
        {
            return Ok(ApiResponseDto<RepairTicketResponseDto>.Failure("Ticket not found", null));
        }

        return Ok(ApiResponseDto<RepairTicketResponseDto>.Success(result, "Ticket retrieved successfully"));
    }

    /// <summary>
    /// Get all tickets
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<RepairTicketResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<RepairTicketResponseDto>>>> GetAllTickets()
    {
        var result = await _ticketService.GetAllTicketsAsync();
        return Ok(ApiResponseDto<IEnumerable<RepairTicketResponseDto>>.Success(result, "Tickets retrieved successfully"));
    }

    /// <summary>
    /// Get tickets by status
    /// </summary>
    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<RepairTicketResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<RepairTicketResponseDto>>>> GetTicketsByStatus(string status)
    {
        var result = await _ticketService.GetTicketsByStatusAsync(status);
        return Ok(ApiResponseDto<IEnumerable<RepairTicketResponseDto>>.Success(result, "Tickets retrieved successfully"));
    }

    /// <summary>
    /// Get tickets by priority
    /// </summary>
    [HttpGet("priority/{priority}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<RepairTicketResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<RepairTicketResponseDto>>>> GetTicketsByPriority(string priority)
    {
        var result = await _ticketService.GetTicketsByPriorityAsync(priority);
        return Ok(ApiResponseDto<IEnumerable<RepairTicketResponseDto>>.Success(result, "Tickets retrieved successfully"));
    }

    /// <summary>
    /// Get tickets by employee ID
    /// </summary>
    [HttpGet("employee/{employeeId}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<RepairTicketResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<RepairTicketResponseDto>>>> GetTicketsByEmployee(Guid employeeId)
    {
        var result = await _ticketService.GetTicketsByEmployeeIdAsync(employeeId);
        return Ok(ApiResponseDto<IEnumerable<RepairTicketResponseDto>>.Success(result, "Tickets retrieved successfully"));
    }

    /// <summary>
    /// Get overdue tickets
    /// </summary>
    [HttpGet("overdue")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<RepairTicketResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<RepairTicketResponseDto>>>> GetOverdueTickets()
    {
        var result = await _ticketService.GetOverdueTicketsAsync();
        return Ok(ApiResponseDto<IEnumerable<RepairTicketResponseDto>>.Success(result, "Overdue tickets retrieved successfully"));
    }

    /// <summary>
    /// Search tickets
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<RepairTicketResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<RepairTicketResponseDto>>>> SearchTickets([FromQuery] string term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return Ok(ApiResponseDto<IEnumerable<RepairTicketResponseDto>>.Failure("Search term is required", null));
        }

        var result = await _ticketService.SearchTicketsAsync(term);
        return Ok(ApiResponseDto<IEnumerable<RepairTicketResponseDto>>.Success(result, "Search completed successfully"));
    }

    /// <summary>
    /// Update ticket
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<RepairTicketResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<RepairTicketResponseDto>>> UpdateTicket(Guid id, [FromBody] UpdateRepairTicketRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<RepairTicketResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _ticketService.UpdateTicketAsync(id, request);
        return Ok(ApiResponseDto<RepairTicketResponseDto>.Success(result, "Ticket updated successfully"));
    }

    /// <summary>
    /// Update ticket status
    /// </summary>
    [HttpPut("{id}/status")]
    [ProducesResponseType(typeof(ApiResponseDto<RepairTicketResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<RepairTicketResponseDto>>> UpdateTicketStatus(Guid id, [FromBody] string status)
    {
        var result = await _ticketService.UpdateTicketStatusAsync(id, status);
        return Ok(ApiResponseDto<RepairTicketResponseDto>.Success(result, "Ticket status updated successfully"));
    }

    /// <summary>
    /// Assign ticket to employee
    /// </summary>
    [HttpPost("{id}/assign/{employeeId}")]
    [ProducesResponseType(typeof(ApiResponseDto<RepairTicketResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<RepairTicketResponseDto>>> AssignTicket(Guid id, Guid employeeId)
    {
        var result = await _ticketService.AssignTicketAsync(id, employeeId);
        return Ok(ApiResponseDto<RepairTicketResponseDto>.Success(result, "Ticket assigned successfully"));
    }

    /// <summary>
    /// Delete ticket
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> DeleteTicket(Guid id)
    {
        var deleted = await _ticketService.DeleteTicketAsync(id);
        if (!deleted)
        {
            return Ok(ApiResponseDto<object>.Failure("Ticket not found", null));
        }

        return Ok(ApiResponseDto<object?>.Success(null, "Ticket deleted successfully"));
    }
}

