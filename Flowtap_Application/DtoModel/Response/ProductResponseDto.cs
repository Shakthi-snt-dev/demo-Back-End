namespace Flowtap_Application.DtoModel.Response;

public class ProductResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? SKU { get; set; }
    public string? Brand { get; set; }
    public string? Description { get; set; }
    public Guid? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public Guid? SubCategoryId { get; set; }
    public string? SubCategoryName { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public string ProductType { get; set; } = string.Empty;
    public bool TrackSerials { get; set; }
    public bool IsActive { get; set; }
    public Guid? SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public string? Condition { get; set; }
    public string? InventoryValuationMethod { get; set; }
    public decimal MinimumPrice { get; set; }
    public string? TaxClass { get; set; }
    public bool ShowOnPOS { get; set; }
    public string? UPCCode { get; set; }
    public List<ProductVariantResponseDto> Variants { get; set; } = new();
    public decimal ProfitMargin { get; set; }
    public decimal ProfitAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class InventoryItemResponseDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public ProductResponseDto? Product { get; set; }
    public Guid StoreId { get; set; }
    public int QuantityOnHand { get; set; }
    public int QuantityReserved { get; set; }
    public int QuantityAvailable { get; set; }
    public int ReorderLevel { get; set; }
    public string? Location { get; set; }
    public bool IsInStock { get; set; }
    public bool IsBelowReorderLevel { get; set; }
    public int SerialCount { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ProductVariantResponseDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string? AttributeName { get; set; }
    public string? AttributeValue { get; set; }
}

public class ProductCategoryResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<ProductSubCategoryResponseDto> SubCategories { get; set; } = new();
}

public class ProductSubCategoryResponseDto
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class SerialNumberResponseDto
{
    public Guid Id { get; set; }
    public Guid InventoryItemId { get; set; }
    public string Serial { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class InventoryTransactionResponseDto
{
    public Guid Id { get; set; }
    public Guid InventoryItemId { get; set; }
    public string Type { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? ReferenceType { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class StockTransferResponseDto
{
    public Guid Id { get; set; }
    public Guid FromStoreId { get; set; }
    public Guid ToStoreId { get; set; }
    public string Status { get; set; } = string.Empty;
    public Guid? RequestedById { get; set; }
    public Guid? ApprovedById { get; set; }
    public List<StockTransferItemResponseDto> Items { get; set; } = new();
    public int TotalQuantity { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class StockTransferItemResponseDto
{
    public Guid Id { get; set; }
    public Guid StockTransferId { get; set; }
    public Guid InventoryItemId { get; set; }
    public InventoryItemResponseDto? InventoryItem { get; set; }
    public int Quantity { get; set; }
}

