using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Sales.Interfaces;
using Flowtap_Domain.BoundedContexts.Inventory.Interfaces;
using Flowtap_Domain.DtoModel;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<DashboardService> _logger;

    public DashboardService(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IProductRepository productRepository,
        ILogger<DashboardService> logger)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<DashboardResponseDto> GetDashboardDataAsync(Guid? storeId = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        var end = endDate ?? DateTime.UtcNow;
        var start = startDate ?? end.AddDays(-30);

        var stats = await GetDashboardStatsAsync(storeId);
        var salesChartData = await GetSalesChartDataAsync(storeId, 7);
        var topProducts = await GetTopProductsAsync(storeId, 5);
        var recentOrders = await GetRecentOrdersAsync(storeId, 10);

        return new DashboardResponseDto
        {
            Stats = stats,
            SalesChartData = salesChartData,
            TopProducts = topProducts,
            RecentOrders = recentOrders
        };
    }

    public async Task<DashboardStatsResponseDto> GetDashboardStatsAsync(Guid? storeId = null)
    {
        var endDate = DateTime.UtcNow;
        var startDate = endDate.AddDays(-30);
        var previousStartDate = startDate.AddDays(-30);
        var previousEndDate = startDate;

        // Get current period orders
        var currentOrders = storeId.HasValue
            ? (await _orderRepository.GetByDateRangeAsync(startDate, endDate))
                .Where(o => o.StoreId == storeId.Value && o.Status == "completed")
            : (await _orderRepository.GetByDateRangeAsync(startDate, endDate))
                .Where(o => o.Status == "completed");

        // Get previous period orders
        var previousOrders = storeId.HasValue
            ? (await _orderRepository.GetByDateRangeAsync(previousStartDate, previousEndDate))
                .Where(o => o.StoreId == storeId.Value && o.Status == "completed")
            : (await _orderRepository.GetByDateRangeAsync(previousStartDate, previousEndDate))
                .Where(o => o.Status == "completed");

        var currentRevenue = currentOrders.Sum(o => o.Total);
        var previousRevenue = previousOrders.Sum(o => o.Total);
        var revenueChange = previousRevenue > 0
            ? ((currentRevenue - previousRevenue) / previousRevenue) * 100
            : 0;

        var currentOrderCount = currentOrders.Count();
        var previousOrderCount = previousOrders.Count();
        var ordersChange = previousOrderCount > 0
            ? ((currentOrderCount - previousOrderCount) / (decimal)previousOrderCount) * 100
            : 0;

        var allProducts = await _productRepository.GetAllAsync();
        var productsCount = allProducts.Count();
        var productsChange = 0m; // Would need historical data

        var allCustomers = await _customerRepository.GetAllAsync();
        var customersCount = allCustomers.Count();
        var customersChange = 0m; // Would need historical data

        return new DashboardStatsResponseDto
        {
            TotalRevenue = currentRevenue,
            RevenueChange = revenueChange,
            TotalOrders = currentOrderCount,
            OrdersChange = ordersChange,
            TotalProducts = productsCount,
            ProductsChange = productsChange,
            TotalCustomers = customersCount,
            CustomersChange = customersChange
        };
    }

    public async Task<List<SalesChartDataDto>> GetSalesChartDataAsync(Guid? storeId = null, int days = 7)
    {
        var endDate = DateTime.UtcNow.Date;
        var startDate = endDate.AddDays(-days);

        var orders = storeId.HasValue
            ? (await _orderRepository.GetByDateRangeAsync(startDate, endDate))
                .Where(o => o.StoreId == storeId.Value && o.Status == "completed")
            : (await _orderRepository.GetByDateRangeAsync(startDate, endDate))
                .Where(o => o.Status == "completed");

        var groupedByDate = orders
            .GroupBy(o => o.CreatedAt.Date)
            .Select(g => new SalesChartDataDto
            {
                Date = g.Key.ToString("ddd"), // Mon, Tue, etc.
                Sales = g.Sum(o => o.Total),
                Orders = g.Count()
            })
            .OrderBy(d => d.Date)
            .ToList();

        // Fill in missing days with zero values
        var result = new List<SalesChartDataDto>();
        for (int i = 0; i < days; i++)
        {
            var date = endDate.AddDays(-i);
            var dayData = groupedByDate.FirstOrDefault(d => d.Date == date.ToString("ddd"));
            result.Add(dayData ?? new SalesChartDataDto
            {
                Date = date.ToString("ddd"),
                Sales = 0,
                Orders = 0
            });
        }

        return result.OrderBy(d => d.Date).ToList();
    }

    public async Task<List<TopProductDto>> GetTopProductsAsync(Guid? storeId = null, int limit = 5)
    {
        var endDate = DateTime.UtcNow;
        var startDate = endDate.AddDays(-30);

        var orders = storeId.HasValue
            ? (await _orderRepository.GetByDateRangeAsync(startDate, endDate))
                .Where(o => o.StoreId == storeId.Value && o.Status == "completed")
            : (await _orderRepository.GetByDateRangeAsync(startDate, endDate))
                .Where(o => o.Status == "completed");

        var productSales = orders
            .SelectMany(o => o.Items)
            .GroupBy(i => new { i.ProductId, i.ProductName })
            .Select(g => new TopProductDto
            {
                Name = g.Key.ProductName,
                Sold = g.Sum(i => i.Quantity),
                Revenue = g.Sum(i => i.Total)
            })
            .OrderByDescending(p => p.Revenue)
            .Take(limit)
            .ToList();

        return productSales;
    }

    public async Task<List<RecentOrderDto>> GetRecentOrdersAsync(Guid? storeId = null, int limit = 10)
    {
        var orders = storeId.HasValue
            ? (await _orderRepository.GetByStoreIdAsync(storeId.Value))
                .OrderByDescending(o => o.CreatedAt)
                .Take(limit)
            : (await _orderRepository.GetAllAsync())
                .OrderByDescending(o => o.CreatedAt)
                .Take(limit);

        return orders.Select(o => new RecentOrderDto
        {
            OrderNumber = o.OrderNumber,
            CustomerName = o.CustomerName ?? "Walk-in Customer",
            Amount = o.Total,
            Status = o.Status,
            CreatedAt = o.CreatedAt
        }).ToList();
    }
}

