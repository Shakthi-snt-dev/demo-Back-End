using System.ComponentModel.DataAnnotations;

namespace Flowtap_Application.DtoModel.Request;

public class CreateDeviceCategoryRequestDto
{
    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    public string? PictureUrl { get; set; } // Will be set after file upload

    public bool IsLaborBillable { get; set; } = false;
}

public class UpdateDeviceCategoryRequestDto
{
    [MaxLength(150)]
    public string? Name { get; set; }

    public string? PictureUrl { get; set; }

    public bool? IsLaborBillable { get; set; }
}

public class CreateDeviceBrandRequestDto
{
    [Required]
    public Guid CategoryId { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;
}

public class CreateDeviceModelRequestDto
{
    [Required]
    public Guid BrandId { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;
}

public class CreateDeviceVariantRequestDto
{
    [Required]
    public Guid ModelId { get; set; }

    [Required, MaxLength(100)]
    public string Attribute { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Value { get; set; } = string.Empty;
}

public class CreateDeviceProblemRequestDto
{
    [Required]
    public Guid ModelId { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public Guid? ServiceId { get; set; }

    public int? WarrantyDays { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? RetailPrice { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? SalePrice { get; set; }

    [MaxLength(50)]
    public string? TaxClass { get; set; }

    public bool ShowOnPOS { get; set; } = true;

    public bool ShowOnWidget { get; set; } = true;
}

public class CreateServiceRequestDto
{
    [Required]
    public Guid StoreId { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal BasePrice { get; set; }

    public bool IsDeviceSpecific { get; set; } = false;

    public List<ServicePartRequestDto> Parts { get; set; } = new();

    public List<ServiceLaborRequestDto> Labor { get; set; } = new();

    public List<ServiceWarrantyRequestDto> Warranties { get; set; } = new();
}

public class ServicePartRequestDto
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; } = 1;
}

public class ServiceLaborRequestDto
{
    [MaxLength(200)]
    public string Label { get; set; } = "Labor";

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Cost { get; set; }
}

public class ServiceWarrantyRequestDto
{
    [Required]
    [Range(0, int.MaxValue)]
    public int WarrantyDays { get; set; } = 30;
}

public class UpdateServiceRequestDto
{
    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? BasePrice { get; set; }

    public bool? IsDeviceSpecific { get; set; }

    public bool? IsActive { get; set; }

    [MaxLength(50)]
    public string? TaxClass { get; set; }

    public bool? ShowOnPOS { get; set; }

    public bool? ShowOnWidget { get; set; }
}

public class CreatePreCheckItemRequestDto
{
    [Required]
    public Guid StoreId { get; set; }

    [Required, MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    public int DisplayOrder { get; set; } = 0;
}

public class UpdatePreCheckItemRequestDto
{
    [MaxLength(200)]
    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public int? DisplayOrder { get; set; }
}

