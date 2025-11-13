namespace FlowTap.Api.Models;

public enum CommissionType
{
    Sales,
    Repairs,
    Both
}

public class CommissionRule
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public CommissionType Type { get; set; }
    public decimal Percentage { get; set; }
    public string? CalculateOn { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

