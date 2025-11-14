using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(
        IDashboardService dashboardService,
        ILogger<DashboardController> logger)
    {
        _dashboardService = dashboardService;
        _logger = logger;
    }

    /// <summary>
    /// Get complete dashboard data
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<DashboardResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<DashboardResponseDto>>> GetDashboard(
        [FromQuery] Guid? storeId = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var result = await _dashboardService.GetDashboardDataAsync(storeId, startDate, endDate);
        return Ok(ApiResponseDto<DashboardResponseDto>.Success(result, "Dashboard data retrieved successfully"));
    }

    /// <summary>
    /// Get dashboard statistics
    /// </summary>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(ApiResponseDto<DashboardStatsResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<DashboardStatsResponseDto>>> GetStats([FromQuery] Guid? storeId = null)
    {
        var result = await _dashboardService.GetDashboardStatsAsync(storeId);
        return Ok(ApiResponseDto<DashboardStatsResponseDto>.Success(result, "Statistics retrieved successfully"));
    }

    /// <summary>
    /// Get sales chart data
    /// </summary>
    [HttpGet("sales-chart")]
    [ProducesResponseType(typeof(ApiResponseDto<List<SalesChartDataDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<List<SalesChartDataDto>>>> GetSalesChart(
        [FromQuery] Guid? storeId = null,
        [FromQuery] int days = 7)
    {
        var result = await _dashboardService.GetSalesChartDataAsync(storeId, days);
        return Ok(ApiResponseDto<List<SalesChartDataDto>>.Success(result, "Sales chart data retrieved successfully"));
    }

    /// <summary>
    /// Get top products
    /// </summary>
    [HttpGet("top-products")]
    [ProducesResponseType(typeof(ApiResponseDto<List<TopProductDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<List<TopProductDto>>>> GetTopProducts(
        [FromQuery] Guid? storeId = null,
        [FromQuery] int limit = 5)
    {
        var result = await _dashboardService.GetTopProductsAsync(storeId, limit);
        return Ok(ApiResponseDto<List<TopProductDto>>.Success(result, "Top products retrieved successfully"));
    }

    /// <summary>
    /// Get recent orders
    /// </summary>
    [HttpGet("recent-orders")]
    [ProducesResponseType(typeof(ApiResponseDto<List<RecentOrderDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<List<RecentOrderDto>>>> GetRecentOrders(
        [FromQuery] Guid? storeId = null,
        [FromQuery] int limit = 10)
    {
        var result = await _dashboardService.GetRecentOrdersAsync(storeId, limit);
        return Ok(ApiResponseDto<List<RecentOrderDto>>.Success(result, "Recent orders retrieved successfully"));
    }
}

