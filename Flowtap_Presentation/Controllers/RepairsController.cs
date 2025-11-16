using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Repairs Controller - Alias for RepairTicket operations
/// </summary>
[ApiController]
[Route("api/repairs")]
public class RepairsController : ControllerBase
{
    private readonly IRepairTicketService _ticketService;
    private readonly ILogger<RepairsController> _logger;

    public RepairsController(
        IRepairTicketService ticketService,
        ILogger<RepairsController> logger)
    {
        _ticketService = ticketService;
        _logger = logger;
    }

    /// <summary>
    /// Get all repair tickets
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<RepairTicketResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<RepairTicketResponseDto>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10,
        [FromQuery] string? status = null)
    {
        var result = await _ticketService.GetAllTicketsAsync();
        
        // Apply status filter if provided
        if (!string.IsNullOrEmpty(status))
        {
            result = result.Where(t => t.Status?.Equals(status, StringComparison.OrdinalIgnoreCase) == true).ToList();
        }

        // Apply pagination
        var paginatedResult = result.Skip((page - 1) * limit).Take(limit).ToList();
        
        return Ok(ApiResponseDto<IEnumerable<RepairTicketResponseDto>>.Success(paginatedResult, "Repair tickets retrieved successfully"));
    }

    /// <summary>
    /// Get repair ticket by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<RepairTicketResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<RepairTicketResponseDto>>> GetById(Guid id)
    {
        var result = await _ticketService.GetTicketByIdAsync(id);
        return Ok(ApiResponseDto<RepairTicketResponseDto>.Success(result, "Repair ticket retrieved successfully"));
    }

    /// <summary>
    /// Create a new repair ticket
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<RepairTicketResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<RepairTicketResponseDto>>> Create([FromBody] CreateRepairTicketRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<RepairTicketResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _ticketService.CreateTicketAsync(request);
        return Ok(ApiResponseDto<RepairTicketResponseDto>.Success(result, "Repair ticket created successfully"));
    }

    /// <summary>
    /// Update repair ticket
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<RepairTicketResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<RepairTicketResponseDto>>> Update(
        Guid id,
        [FromBody] UpdateRepairTicketRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<RepairTicketResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _ticketService.UpdateTicketAsync(id, request);
        return Ok(ApiResponseDto<RepairTicketResponseDto>.Success(result, "Repair ticket updated successfully"));
    }

    /// <summary>
    /// Update repair ticket status
    /// </summary>
    [HttpPatch("{id}/status")]
    [ProducesResponseType(typeof(ApiResponseDto<RepairTicketResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<RepairTicketResponseDto>>> UpdateStatus(
        Guid id,
        [FromBody] UpdateStatusRequestDto request)
    {
        var result = await _ticketService.UpdateTicketStatusAsync(id, request.Status);
        return Ok(ApiResponseDto<RepairTicketResponseDto>.Success(result, "Repair ticket status updated successfully"));
    }

    /// <summary>
    /// Delete repair ticket
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> Delete(Guid id)
    {
        var deleted = await _ticketService.DeleteTicketAsync(id);
        if (!deleted)
        {
            return Ok(ApiResponseDto<object>.Failure("Repair ticket not found", null));
        }

        return Ok(ApiResponseDto<object?>.Success(null, "Repair ticket deleted successfully"));
    }
}

// DTO for status update
public class UpdateStatusRequestDto
{
    public string Status { get; set; } = string.Empty;
}

