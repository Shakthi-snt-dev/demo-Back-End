namespace Flowtap_Application.DtoModel.Response;

public class RepairTicketResponseDto
{
    public Guid Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public Guid StoreId { get; set; }
    public Guid? CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }
    public string? CustomerEmail { get; set; }
    public string Device { get; set; } = string.Empty;
    public string Issue { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public decimal EstimatedCost { get; set; }
    public decimal DepositPaid { get; set; }
    public decimal BalanceDue { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? AssignedToEmployeeId { get; set; }
    public string? Notes { get; set; }
    public bool IsOverdue { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

