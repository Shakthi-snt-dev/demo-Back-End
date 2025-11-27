using System.ComponentModel.DataAnnotations;
using Flowtap_Domain.SharedKernel.Enums;

namespace Flowtap_Application.DtoModel.Request;

public class CreateSupplierRequestDto
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(320)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(100)]
    public string? ContactPerson { get; set; }
}

public class UpdateSupplierRequestDto
{
    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(320)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(100)]
    public string? ContactPerson { get; set; }

    public bool? IsActive { get; set; }
}

public class CreatePurchaseOrderRequestDto
{
    [MaxLength(100)]
    public string? PONumber { get; set; }

    [Required]
    public Guid SupplierId { get; set; }

    [Required]
    public Guid StoreId { get; set; }

    public List<PurchaseOrderLineRequestDto> Lines { get; set; } = new();
}

public class PurchaseOrderLineRequestDto
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal UnitCost { get; set; }
}

public class UpdatePurchaseOrderRequestDto
{
    public PurchaseOrderStatus? Status { get; set; }
}

public class CreateSupplierReturnRequestDto
{
    [Required]
    public Guid SupplierId { get; set; }

    [Required]
    public Guid StoreId { get; set; }

    public List<SupplierReturnItemRequestDto> Items { get; set; } = new();
}

public class SupplierReturnItemRequestDto
{
    [Required]
    public Guid InventoryItemId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}

