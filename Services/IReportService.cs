namespace FlowTap.Api.Services;

public interface IReportService
{
    Task<SalesReportDto> GetSalesReportAsync(DateTime? startDate, DateTime? endDate);
    Task<RepairsReportDto> GetRepairsReportAsync(DateTime? startDate, DateTime? endDate);
    Task<InventoryReportDto> GetInventoryReportAsync();
    Task<TaxReportDto> GetTaxReportAsync(DateTime? startDate, DateTime? endDate);
    Task<CommissionsReportDto> GetCommissionsReportAsync(DateTime? startDate, DateTime? endDate);
}

public class SalesReportDto
{
    public decimal TotalSales { get; set; }
    public int TotalTransactions { get; set; }
    public decimal AverageTransaction { get; set; }
    public List<SalesByCategoryDto> SalesByCategory { get; set; } = new();
}

public class SalesByCategoryDto
{
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int Count { get; set; }
}

public class RepairsReportDto
{
    public int TotalTickets { get; set; }
    public int Completed { get; set; }
    public int InProgress { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<RepairsByStatusDto> ByStatus { get; set; } = new();
}

public class RepairsByStatusDto
{
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class InventoryReportDto
{
    public int TotalProducts { get; set; }
    public int LowStockItems { get; set; }
    public decimal TotalValue { get; set; }
    public List<InventoryByCategoryDto> ByCategory { get; set; } = new();
}

public class InventoryByCategoryDto
{
    public string Category { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal Value { get; set; }
}

public class TaxReportDto
{
    public decimal TotalTax { get; set; }
    public decimal TotalSales { get; set; }
    public List<TaxByPeriodDto> ByPeriod { get; set; } = new();
}

public class TaxByPeriodDto
{
    public DateTime Period { get; set; }
    public decimal Tax { get; set; }
    public decimal Sales { get; set; }
}

public class CommissionsReportDto
{
    public decimal TotalCommissions { get; set; }
    public List<CommissionByEmployeeDto> ByEmployee { get; set; } = new();
}

public class CommissionByEmployeeDto
{
    public string EmployeeName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

