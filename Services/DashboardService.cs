using FlowTap.Api.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace FlowTap.Api.Services;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;
    private readonly IDatabase? _redis;

    public DashboardService(ApplicationDbContext context, IConnectionMultiplexer? redis = null)
    {
        _context = context;
        _redis = redis?.GetDatabase();
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync()
    {
        const string cacheKey = "dashboard:summary";
        if (_redis != null)
        {
            var cached = await _redis.StringGetAsync(cacheKey);
            if (cached.HasValue)
            {
                return System.Text.Json.JsonSerializer.Deserialize<DashboardSummaryDto>(cached!)!;
            }
        }

        var today = DateTime.UtcNow.Date;
        var summary = new DashboardSummaryDto
        {
            TotalSales = await _context.Sales.SumAsync(s => s.Total),
            TotalRepairs = await _context.RepairTickets.CountAsync(),
            LowStockItems = await _context.Products.CountAsync(p => p.Stock <= p.LowStockAlert),
            ActiveTickets = await _context.RepairTickets.CountAsync(r => 
                r.Status != Models.RepairStatus.Delivered && r.Status != Models.RepairStatus.Cancelled),
            TodayRevenue = await _context.Sales
                .Where(s => s.CreatedAt.Date == today)
                .SumAsync(s => s.Total),
            TodaySales = await _context.Sales
                .Where(s => s.CreatedAt.Date == today)
                .CountAsync()
        };

        if (_redis != null)
        {
            await _redis.StringSetAsync(cacheKey, System.Text.Json.JsonSerializer.Serialize(summary), TimeSpan.FromMinutes(5));
        }
        return summary;
    }

    public async Task<List<SalesTrendDto>> GetSalesTrendsAsync(DateTime? startDate, DateTime? endDate)
    {
        var start = startDate ?? DateTime.UtcNow.AddDays(-30);
        var end = endDate ?? DateTime.UtcNow;

        var trends = await _context.Sales
            .Where(s => s.CreatedAt >= start && s.CreatedAt <= end)
            .GroupBy(s => s.CreatedAt.Date)
            .Select(g => new SalesTrendDto
            {
                Date = g.Key,
                Amount = g.Sum(s => s.Total),
                Count = g.Count()
            })
            .OrderBy(t => t.Date)
            .ToListAsync();

        return trends;
    }

    public async Task<RepairsStatsDto> GetRepairsStatsAsync()
    {
        return new RepairsStatsDto
        {
            Total = await _context.RepairTickets.CountAsync(),
            New = await _context.RepairTickets.CountAsync(r => r.Status == Models.RepairStatus.New),
            InProgress = await _context.RepairTickets.CountAsync(r => 
                r.Status == Models.RepairStatus.InProgress || r.Status == Models.RepairStatus.Diagnosing),
            Completed = await _context.RepairTickets.CountAsync(r => r.Status == Models.RepairStatus.Delivered)
        };
    }
}

