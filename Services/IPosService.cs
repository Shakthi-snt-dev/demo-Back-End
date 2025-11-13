using FlowTap.Api.Models;

namespace FlowTap.Api.Services;

public interface IPosService
{
    Task<Sale> CreateSaleAsync(CreateSaleRequest request);
    Task<List<SaleDto>> GetSalesAsync(DateTime? startDate, DateTime? endDate);
    Task<SaleDto> GetSaleByIdAsync(Guid id);
    Task<bool> ProcessRefundAsync(Guid saleId);
}

public class CreateSaleRequest
{
    public Guid StoreId { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? EmployeeId { get; set; }
    public List<SaleItemRequest> Items { get; set; } = new();
    public decimal Discount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
}

public class SaleItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class SaleDto
{
    public Guid Id { get; set; }
    public Guid StoreId { get; set; }
    public string? CustomerName { get; set; }
    public string? EmployeeName { get; set; }
    public decimal Total { get; set; }
    public decimal Tax { get; set; }
    public decimal Discount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<SaleItemDto> Items { get; set; } = new();
}

public class SaleItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }
}

