namespace Flowtap_Domain.DtoModel;

public class DashboardStatsResponseDto
{
    public decimal TotalRevenue { get; set; }
    public decimal RevenueChange { get; set; } // percentage
    public int TotalOrders { get; set; }
    public decimal OrdersChange { get; set; } // percentage
    public int TotalProducts { get; set; }
    public decimal ProductsChange { get; set; } // percentage
    public int TotalCustomers { get; set; }
    public decimal CustomersChange { get; set; } // percentage
}

public class SalesChartDataDto
{
    public string Date { get; set; } = string.Empty;
    public decimal Sales { get; set; }
    public int Orders { get; set; }
}

public class TopProductDto
{
    public string Name { get; set; } = string.Empty;
    public int Sold { get; set; }
    public decimal Revenue { get; set; }
}

public class RecentOrderDto
{
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class DashboardResponseDto
{
    public DashboardStatsResponseDto Stats { get; set; } = new DashboardStatsResponseDto();
    public List<SalesChartDataDto> SalesChartData { get; set; } = new List<SalesChartDataDto>();
    public List<TopProductDto> TopProducts { get; set; } = new List<TopProductDto>();
    public List<RecentOrderDto> RecentOrders { get; set; } = new List<RecentOrderDto>();
}

