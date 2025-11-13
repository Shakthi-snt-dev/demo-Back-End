namespace FlowTap.Api.Models;

public class Customer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public Guid? StoreId { get; set; }
    public int LoyaltyPoints { get; set; } = 0;
    public decimal TotalSpent { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    public ICollection<RepairTicket> RepairTickets { get; set; } = new List<RepairTicket>();
    public ICollection<CustomerHistory> History { get; set; } = new List<CustomerHistory>();
}

