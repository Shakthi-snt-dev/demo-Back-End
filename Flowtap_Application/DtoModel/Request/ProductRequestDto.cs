using System.ComponentModel.DataAnnotations;
using Flowtap_Application.DtoModel;

namespace Flowtap_Application.DtoModel.Request;

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

