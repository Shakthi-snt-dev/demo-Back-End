namespace Flowtap_Application.DtoModel.Response;

public class OrderResponseDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public Guid StoreId { get; set; }
    public List<OrderItemResponseDto> Items { get; set; } = new List<OrderItemResponseDto>();
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class OrderItemResponseDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }
}

