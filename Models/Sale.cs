namespace FlowTap.Api.Models;

public class Sale
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StoreId { get; set; }
    public Store Store { get; set; } = null!;
    public Guid? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public Guid? EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public decimal Total { get; set; }
    public decimal Tax { get; set; }
    public decimal Discount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

