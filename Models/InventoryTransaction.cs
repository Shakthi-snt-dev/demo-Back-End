namespace FlowTap.Api.Models;

public enum InventoryTransactionType
{
    Purchase,
    Sale,
    Return,
    Adjustment,
    Transfer
}

public class InventoryTransaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public Guid? StoreId { get; set; }
    public int Quantity { get; set; }
    public InventoryTransactionType Type { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

