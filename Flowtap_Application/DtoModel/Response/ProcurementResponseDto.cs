namespace Flowtap_Application.DtoModel.Response;

public class SupplierResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? ContactPerson { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class PurchaseOrderResponseDto
{
    public Guid Id { get; set; }
    public string? PONumber { get; set; }
    public Guid SupplierId { get; set; }
    public SupplierResponseDto? Supplier { get; set; }
    public Guid StoreId { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<PurchaseOrderLineResponseDto> Lines { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PurchaseOrderLineResponseDto
{
    public Guid Id { get; set; }
    public Guid PurchaseOrderId { get; set; }
    public Guid ProductId { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
}

public class SupplierReturnResponseDto
{
    public Guid Id { get; set; }
    public Guid SupplierId { get; set; }
    public SupplierResponseDto? Supplier { get; set; }
    public Guid StoreId { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<SupplierReturnItemResponseDto> Items { get; set; } = new();
    public int TotalQuantity { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SupplierReturnItemResponseDto
{
    public Guid Id { get; set; }
    public Guid SupplierReturnId { get; set; }
    public Guid InventoryItemId { get; set; }
    public InventoryItemResponseDto? InventoryItem { get; set; }
    public int Quantity { get; set; }
}

