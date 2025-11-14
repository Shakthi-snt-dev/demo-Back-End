using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.DtoModel;

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

