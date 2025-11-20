using System.ComponentModel.DataAnnotations;

namespace Flowtap_Application.DtoModel.Request;

public class CreateRepairTicketRequestDto
{
    [Required]
    public Guid StoreId { get; set; }

    public Guid? CustomerId { get; set; } // Optional link to existing customer

    [Required, MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? CustomerPhone { get; set; }

    [MaxLength(320)]
    [EmailAddress]
    public string? CustomerEmail { get; set; }

    [Required, MaxLength(200)]
    public string Device { get; set; } = string.Empty;

    [Required]
    public string Issue { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Priority { get; set; } = "medium"; // low, medium, high, urgent

    [Range(0, double.MaxValue)]
    public decimal EstimatedCost { get; set; } = 0;

    [Range(0, double.MaxValue)]
    public decimal DepositPaid { get; set; } = 0;

    public DateTime? DueDate { get; set; }

    public Guid? AssignedToEmployeeId { get; set; }

    public string? Notes { get; set; }
}

public class UpdateRepairTicketRequestDto
{
    [MaxLength(200)]
    public string? CustomerName { get; set; }

    [MaxLength(20)]
    public string? CustomerPhone { get; set; }

    [MaxLength(320)]
    [EmailAddress]
    public string? CustomerEmail { get; set; }

    [MaxLength(200)]
    public string? Device { get; set; }

    public string? Issue { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; } // pending, in-progress, completed, cancelled

    [MaxLength(50)]
    public string? Priority { get; set; } // low, medium, high, urgent

    [Range(0, double.MaxValue)]
    public decimal? EstimatedCost { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? DepositPaid { get; set; }

    public DateTime? DueDate { get; set; }

    public Guid? AssignedToEmployeeId { get; set; }

    public string? Notes { get; set; }
}

