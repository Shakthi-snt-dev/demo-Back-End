namespace Flowtap_Application.DtoModel.Response;

public class ProductResponseDto
{
    public Guid Id { get; set; }
    public Guid StoreId { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public int Stock { get; set; }
    public int MinStock { get; set; }
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
    public bool IsLowStock { get; set; }
    public bool IsInStock { get; set; }
    public decimal ProfitMargin { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

