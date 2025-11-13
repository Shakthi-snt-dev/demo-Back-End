namespace FlowTap.Api.Services;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetSummaryAsync();
    Task<List<SalesTrendDto>> GetSalesTrendsAsync(DateTime? startDate, DateTime? endDate);
    Task<RepairsStatsDto> GetRepairsStatsAsync();
}

public class DashboardSummaryDto
{
    public decimal TotalSales { get; set; }
    public int TotalRepairs { get; set; }
    public int LowStockItems { get; set; }
    public int ActiveTickets { get; set; }
    public decimal TodayRevenue { get; set; }
    public int TodaySales { get; set; }
}

public class SalesTrendDto
{
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public int Count { get; set; }
}

public class RepairsStatsDto
{
    public int Total { get; set; }
    public int New { get; set; }
    public int InProgress { get; set; }
    public int Completed { get; set; }
}

