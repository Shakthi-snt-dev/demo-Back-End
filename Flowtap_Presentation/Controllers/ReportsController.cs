using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Reports Controller - Alias for Dashboard operations
/// </summary>
[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(
        IDashboardService dashboardService,
        ILogger<ReportsController> logger)
    {
        _dashboardService = dashboardService;
        _logger = logger;
    }

    /// <summary>
    /// Get sales report
    /// </summary>
    [HttpGet("sales")]
    [ProducesResponseType(typeof(ApiResponseDto<DashboardResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<DashboardResponseDto>>> GetSalesReport(
        [FromQuery] string startDate,
        [FromQuery] string endDate,
        [FromQuery] Guid? storeId = null)
    {
        DateTime? start = DateTime.TryParse(startDate, out var sd) ? sd : null;
        DateTime? end = DateTime.TryParse(endDate, out var ed) ? ed : null;

        var result = await _dashboardService.GetDashboardDataAsync(storeId, start, end);
        return Ok(ApiResponseDto<DashboardResponseDto>.Success(result, "Sales report retrieved successfully"));
    }

    /// <summary>
    /// Get inventory report
    /// </summary>
    [HttpGet("inventory")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> GetInventoryReport([FromQuery] Guid? storeId = null)
    {
        // Get dashboard stats which includes inventory information
        var result = await _dashboardService.GetDashboardStatsAsync(storeId);
        return Ok(ApiResponseDto<object>.Success(result, "Inventory report retrieved successfully"));
    }

    /// <summary>
    /// Get repairs report
    /// </summary>
    [HttpGet("repairs")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> GetRepairsReport(
        [FromQuery] string startDate,
        [FromQuery] string endDate,
        [FromQuery] Guid? storeId = null)
    {
        DateTime? start = DateTime.TryParse(startDate, out var sd) ? sd : null;
        DateTime? end = DateTime.TryParse(endDate, out var ed) ? ed : null;

        var result = await _dashboardService.GetDashboardDataAsync(storeId, start, end);
        return Ok(ApiResponseDto<object>.Success(result, "Repairs report retrieved successfully"));
    }

    /// <summary>
    /// Get customer report
    /// </summary>
    [HttpGet("customers")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> GetCustomerReport([FromQuery] Guid? storeId = null)
    {
        var result = await _dashboardService.GetDashboardStatsAsync(storeId);
        return Ok(ApiResponseDto<object>.Success(result, "Customer report retrieved successfully"));
    }

    /// <summary>
    /// Get dashboard statistics (alias for dashboard stats)
    /// </summary>
    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(ApiResponseDto<DashboardStatsResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<DashboardStatsResponseDto>>> GetDashboardStats([FromQuery] Guid? storeId = null)
    {
        var result = await _dashboardService.GetDashboardStatsAsync(storeId);
        return Ok(ApiResponseDto<DashboardStatsResponseDto>.Success(result, "Dashboard statistics retrieved successfully"));
    }
}

