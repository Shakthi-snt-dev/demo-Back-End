using FlowTap.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepairTicketDto = FlowTap.Api.Services.RepairTicketDto;
using CreateRepairTicketRequest = FlowTap.Api.Services.CreateRepairTicketRequest;
using DiagnosticReportDto = FlowTap.Api.Services.DiagnosticReportDto;
using CreateDiagnosticReportRequest = FlowTap.Api.Services.CreateDiagnosticReportRequest;

namespace FlowTap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RepairsController : ControllerBase
{
    private readonly IRepairService _repairService;

    public RepairsController(IRepairService repairService)
    {
        _repairService = repairService;
    }

    [HttpPost("ticket")]
    public async Task<ActionResult<RepairTicketDto>> CreateTicket([FromBody] CreateRepairTicketRequest request)
    {
        try
        {
            var ticket = await _repairService.CreateTicketAsync(request);
            var ticketDto = await _repairService.GetTicketByIdAsync(ticket.Id);
            return Ok(ticketDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("tickets")]
    public async Task<ActionResult<List<RepairTicketDto>>> GetTickets([FromQuery] string? status)
    {
        Models.RepairStatus? repairStatus = null;
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<Models.RepairStatus>(status, out var parsedStatus))
        {
            repairStatus = parsedStatus;
        }

        var tickets = await _repairService.GetTicketsAsync(repairStatus);
        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RepairTicketDto>> GetTicket(Guid id)
    {
        try
        {
            var ticket = await _repairService.GetTicketByIdAsync(id);
            return Ok(ticket);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("diagnostics")]
    public async Task<ActionResult<DiagnosticReportDto>> CreateDiagnosticReport([FromBody] CreateDiagnosticReportRequest request)
    {
        try
        {
            var report = await _repairService.CreateDiagnosticReportAsync(request);
            return Ok(new DiagnosticReportDto
            {
                Id = report.Id,
                IMEI = report.IMEI,
                Model = report.Model,
                OS = report.OS,
                Results = report.Results,
                PassCount = report.PassCount,
                FailCount = report.FailCount,
                CreatedAt = report.CreatedAt
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

