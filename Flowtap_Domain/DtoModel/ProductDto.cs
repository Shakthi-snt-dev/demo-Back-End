using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.DtoModel;

public class CreateProductRequestDto
{
    [Required]
    public Guid StoreId { get; set; }

    [Required, MaxLength(100)]
    public string SKU { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Category { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; } = 0;

    [Range(0, int.MaxValue)]
    public int MinStock { get; set; } = 0;

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; } = 0;

    [Range(0, double.MaxValue)]
    public decimal Cost { get; set; } = 0;
}

public class UpdateProductRequestDto
{
    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(100)]
    public string? Category { get; set; }

    [Range(0, int.MaxValue)]
    public int? Stock { get; set; }

    [Range(0, int.MaxValue)]
    public int? MinStock { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? Price { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? Cost { get; set; }
}

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

