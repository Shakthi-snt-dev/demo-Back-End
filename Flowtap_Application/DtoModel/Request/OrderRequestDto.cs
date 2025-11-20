using System.ComponentModel.DataAnnotations;

namespace Flowtap_Application.DtoModel.Request;

public class CreateOrderRequestDto
{
    public Guid? CustomerId { get; set; }

    [MaxLength(200)]
    public string? CustomerName { get; set; }

    [Required]
    public Guid StoreId { get; set; }

    [Required, MinLength(1)]
    public List<CreateOrderItemRequestDto> Items { get; set; } = new List<CreateOrderItemRequestDto>();

    [Required, MaxLength(50)]
    public string PaymentMethod { get; set; } = string.Empty; // card, cash, other
}

public class CreateOrderItemRequestDto
{
    [Required]
    public Guid ProductId { get; set; }

    [Required, Range(1, int.MaxValue)]
    public int Quantity { get; set; } = 1;

    [Range(0, double.MaxValue)]
    public decimal? Price { get; set; } // Optional, will use product price if not provided
}

public class UpdateOrderRequestDto
{
    [MaxLength(50)]
    public string? Status { get; set; } // pending, processing, completed, cancelled

    [MaxLength(50)]
    public string? PaymentMethod { get; set; }

    public List<UpdateOrderItemRequestDto>? Items { get; set; }
}

public class UpdateOrderItemRequestDto
{
    [Required]
    public Guid ItemId { get; set; }

    [Range(1, int.MaxValue)]
    public int? Quantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? Price { get; set; }
}

