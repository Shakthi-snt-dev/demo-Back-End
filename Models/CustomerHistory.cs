namespace FlowTap.Api.Models;

public enum ReferenceType
{
    Sale,
    Repair,
    Payment,
    Note
}

public class CustomerHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public ReferenceType ReferenceType { get; set; }
    public Guid? ReferenceId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

