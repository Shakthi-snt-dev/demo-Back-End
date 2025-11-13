namespace FlowTap.Api.Models;

public class Store
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Zip { get; set; }
    public string? TimeZone { get; set; }
    public string? Currency { get; set; } = "USD";
    public decimal TaxRate { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    public ICollection<RepairTicket> RepairTickets { get; set; } = new List<RepairTicket>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

