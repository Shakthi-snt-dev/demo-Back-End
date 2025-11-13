namespace FlowTap.Api.Models;

public enum EmployeeStatus
{
    Active,
    Inactive,
    Suspended
}

public class Employee
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public ApplicationRole Role { get; set; } = null!;
    public Guid? StoreId { get; set; }
    public Store? Store { get; set; }
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<CommissionRule> CommissionRules { get; set; } = new List<CommissionRule>();
}

