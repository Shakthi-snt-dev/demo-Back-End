using FlowTap.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DashboardSummaryDto = FlowTap.Api.Services.DashboardSummaryDto;
using SalesTrendDto = FlowTap.Api.Services.SalesTrendDto;
using RepairsStatsDto = FlowTap.Api.Services.RepairsStatsDto;

namespace FlowTap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
    {
        var summary = await _dashboardService.GetSummaryAsync();
        return Ok(summary);
    }

    [HttpGet("sales-trends")]
    public async Task<ActionResult<List<SalesTrendDto>>> GetSalesTrends(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var trends = await _dashboardService.GetSalesTrendsAsync(startDate, endDate);
        return Ok(trends);
    }

    [HttpGet("repairs-stats")]
    public async Task<ActionResult<RepairsStatsDto>> GetRepairsStats()
    {
        var stats = await _dashboardService.GetRepairsStatsAsync();
        return Ok(stats);
    }
}

