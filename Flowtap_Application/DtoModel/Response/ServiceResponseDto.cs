namespace Flowtap_Application.DtoModel.Response;

public class DeviceCategoryResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? PictureUrl { get; set; }
    public bool IsLaborBillable { get; set; }
    public int BrandCount { get; set; }
    public List<DeviceBrandResponseDto> Brands { get; set; } = new();
}

public class DeviceBrandResponseDto
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int ModelCount { get; set; }
    public List<DeviceModelResponseDto> Models { get; set; } = new();
}

public class DeviceModelResponseDto
{
    public Guid Id { get; set; }
    public Guid BrandId { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<DeviceVariantResponseDto> Variants { get; set; } = new();
    public List<DeviceProblemResponseDto> Problems { get; set; } = new();
}

public class DeviceVariantResponseDto
{
    public Guid Id { get; set; }
    public Guid ModelId { get; set; }
    public string Attribute { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class DeviceProblemResponseDto
{
    public Guid Id { get; set; }
    public Guid ModelId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? ServiceId { get; set; }
    public string? ServiceName { get; set; }
    public int? WarrantyDays { get; set; }
    public decimal? RetailPrice { get; set; }
    public decimal? SalePrice { get; set; }
    public string? TaxClass { get; set; }
    public bool ShowOnPOS { get; set; }
    public bool ShowOnWidget { get; set; }
}

public class ServiceResponseDto
{
    public Guid Id { get; set; }
    public Guid StoreId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public bool IsDeviceSpecific { get; set; }
    public bool IsActive { get; set; }
    public string? TaxClass { get; set; }
    public bool ShowOnPOS { get; set; }
    public bool ShowOnWidget { get; set; }
    public decimal TotalCost { get; set; }
    public List<ServicePartResponseDto> Parts { get; set; } = new();
    public List<ServiceLaborResponseDto> Labor { get; set; } = new();
    public List<ServiceWarrantyResponseDto> Warranties { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class SpecialOrderPartResponseDto
{
    public Guid Id { get; set; }
    public Guid StoreId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public Guid? ManufacturerId { get; set; }
    public string? ManufacturerName { get; set; }
    public Guid? DeviceModelId { get; set; }
    public string? DeviceModelName { get; set; }
    public int RequiredQty { get; set; }
    public decimal UnitCost { get; set; }
    public decimal RetailPrice { get; set; }
    public decimal TotalCost { get; set; }
    public Guid SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public string? OrderLink { get; set; }
    public string? TrackingId { get; set; }
    public string? Notes { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public bool IsTaxExclusive { get; set; }
    public bool CreatePurchaseOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ServicePartResponseDto
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public Guid ProductId { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
}

public class ServiceLaborResponseDto
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public string Label { get; set; } = string.Empty;
    public decimal Cost { get; set; }
}

public class ServiceWarrantyResponseDto
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public int WarrantyDays { get; set; }
}

public class PreCheckItemResponseDto
{
    public Guid Id { get; set; }
    public Guid StoreId { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

