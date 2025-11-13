using FlowTap.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace FlowTap.Api.Services;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _context;

    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SalesReportDto> GetSalesReportAsync(DateTime? startDate, DateTime? endDate)
    {
        var start = startDate ?? DateTime.UtcNow.AddDays(-30);
        var end = endDate ?? DateTime.UtcNow;

        var sales = await _context.Sales
            .Where(s => s.CreatedAt >= start && s.CreatedAt <= end)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .ToListAsync();

        var totalSales = sales.Sum(s => s.Total);
        var totalTransactions = sales.Count;
        var averageTransaction = totalTransactions > 0 ? totalSales / totalTransactions : 0;

        var salesByCategory = sales
            .SelectMany(s => s.Items)
            .GroupBy(i => i.Product.Category ?? "Uncategorized")
            .Select(g => new SalesByCategoryDto
            {
                Category = g.Key,
                Amount = g.Sum(i => i.Total),
                Count = g.Count()
            })
            .ToList();

        return new SalesReportDto
        {
            TotalSales = totalSales,
            TotalTransactions = totalTransactions,
            AverageTransaction = averageTransaction,
            SalesByCategory = salesByCategory
        };
    }

    public async Task<RepairsReportDto> GetRepairsReportAsync(DateTime? startDate, DateTime? endDate)
    {
        var start = startDate ?? DateTime.UtcNow.AddDays(-30);
        var end = endDate ?? DateTime.UtcNow;

        var tickets = await _context.RepairTickets
            .Where(t => t.CreatedAt >= start && t.CreatedAt <= end)
            .ToListAsync();

        var byStatus = tickets
            .GroupBy(t => t.Status.ToString())
            .Select(g => new RepairsByStatusDto
            {
                Status = g.Key,
                Count = g.Count()
            })
            .ToList();

        return new RepairsReportDto
        {
            TotalTickets = tickets.Count,
            Completed = tickets.Count(t => t.Status == Models.RepairStatus.Delivered),
            InProgress = tickets.Count(t => t.Status == Models.RepairStatus.InProgress || t.Status == Models.RepairStatus.Diagnosing),
            TotalRevenue = tickets.Where(t => t.Deposit.HasValue).Sum(t => t.Deposit!.Value),
            ByStatus = byStatus
        };
    }

    public async Task<InventoryReportDto> GetInventoryReportAsync()
    {
        var products = await _context.Products.ToListAsync();

        var byCategory = products
            .GroupBy(p => p.Category ?? "Uncategorized")
            .Select(g => new InventoryByCategoryDto
            {
                Category = g.Key,
                Count = g.Count(),
                Value = g.Sum(p => p.Stock * p.Cost)
            })
            .ToList();

        return new InventoryReportDto
        {
            TotalProducts = products.Count,
            LowStockItems = products.Count(p => p.Stock <= p.LowStockAlert),
            TotalValue = products.Sum(p => p.Stock * p.Cost),
            ByCategory = byCategory
        };
    }

    public async Task<TaxReportDto> GetTaxReportAsync(DateTime? startDate, DateTime? endDate)
    {
        var start = startDate ?? DateTime.UtcNow.AddDays(-30);
        var end = endDate ?? DateTime.UtcNow;

        var sales = await _context.Sales
            .Where(s => s.CreatedAt >= start && s.CreatedAt <= end)
            .ToListAsync();

        var byPeriod = sales
            .GroupBy(s => s.CreatedAt.Date)
            .Select(g => new TaxByPeriodDto
            {
                Period = g.Key,
                Tax = g.Sum(s => s.Tax),
                Sales = g.Sum(s => s.Total)
            })
            .OrderBy(p => p.Period)
            .ToList();

        return new TaxReportDto
        {
            TotalTax = sales.Sum(s => s.Tax),
            TotalSales = sales.Sum(s => s.Total),
            ByPeriod = byPeriod
        };
    }

    public async Task<CommissionsReportDto> GetCommissionsReportAsync(DateTime? startDate, DateTime? endDate)
    {
        // Simplified commission calculation
        var start = startDate ?? DateTime.UtcNow.AddDays(-30);
        var end = endDate ?? DateTime.UtcNow;

        var sales = await _context.Sales
            .Where(s => s.CreatedAt >= start && s.CreatedAt <= end && s.EmployeeId.HasValue)
            .Include(s => s.Employee!)
                .ThenInclude(e => e!.CommissionRules)
            .ToListAsync();

        var byEmployee = sales
            .Where(s => s.Employee != null)
            .GroupBy(s => s.Employee!.Name)
            .Select(g => new CommissionByEmployeeDto
            {
                EmployeeName = g.Key ?? "Unknown",
                Amount = g.Sum(s => s.Total * 0.05m) // 5% default commission
            })
            .ToList();

        return new CommissionsReportDto
        {
            TotalCommissions = byEmployee.Sum(e => e.Amount),
            ByEmployee = byEmployee
        };
    }
}

