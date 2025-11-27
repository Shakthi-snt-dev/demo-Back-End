using System.ComponentModel.DataAnnotations;

namespace Flowtap_Application.DtoModel.Request;

public class CreateInventoryItemRequestDto
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid StoreId { get; set; }

    [Range(0, int.MaxValue)]
    public int QuantityOnHand { get; set; } = 0;

    [Range(0, int.MaxValue)]
    public int ReorderLevel { get; set; } = 0;

    [MaxLength(200)]
    public string? Location { get; set; }
}

public class UpdateInventoryItemRequestDto
{
    [Range(0, int.MaxValue)]
    public int? QuantityOnHand { get; set; }

    [Range(0, int.MaxValue)]
    public int? ReorderLevel { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }
}

