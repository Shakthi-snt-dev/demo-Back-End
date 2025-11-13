using FlowTap.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesReportDto = FlowTap.Api.Services.SalesReportDto;
using RepairsReportDto = FlowTap.Api.Services.RepairsReportDto;
using InventoryReportDto = FlowTap.Api.Services.InventoryReportDto;
using TaxReportDto = FlowTap.Api.Services.TaxReportDto;
using CommissionsReportDto = FlowTap.Api.Services.CommissionsReportDto;

namespace FlowTap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("sales")]
    public async Task<ActionResult<SalesReportDto>> GetSalesReport(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var report = await _reportService.GetSalesReportAsync(startDate, endDate);
        return Ok(report);
    }

    [HttpGet("repairs")]
    public async Task<ActionResult<RepairsReportDto>> GetRepairsReport(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var report = await _reportService.GetRepairsReportAsync(startDate, endDate);
        return Ok(report);
    }

    [HttpGet("inventory")]
    public async Task<ActionResult<InventoryReportDto>> GetInventoryReport()
    {
        var report = await _reportService.GetInventoryReportAsync();
        return Ok(report);
    }

    [HttpGet("tax")]
    public async Task<ActionResult<TaxReportDto>> GetTaxReport(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var report = await _reportService.GetTaxReportAsync(startDate, endDate);
        return Ok(report);
    }

    [HttpGet("commissions")]
    public async Task<ActionResult<CommissionsReportDto>> GetCommissionsReport(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var report = await _reportService.GetCommissionsReportAsync(startDate, endDate);
        return Ok(report);
    }
}

