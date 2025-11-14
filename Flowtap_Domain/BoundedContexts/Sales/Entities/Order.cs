using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flowtap_Domain.BoundedContexts.Sales.ValueObjects;

namespace Flowtap_Domain.BoundedContexts.Sales.Entities;

public class Order
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    public Guid? CustomerId { get; set; }

    [MaxLength(200)]
    public string? CustomerName { get; set; }

    public Guid StoreId { get; set; }

    public List<OrderItem> Items { get; set; } = new List<OrderItem>();

    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Tax { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; } = 0;

    [Required, MaxLength(50)]
    public string Status { get; set; } = "pending"; // pending, processing, completed, cancelled

    [Required, MaxLength(50)]
    public string PaymentMethod { get; set; } = string.Empty; // card, cash, other

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Domain methods
    public void AddItem(OrderItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        Items.Add(item);
        RecalculateTotals();
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveItem(Guid itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            throw new InvalidOperationException("Item not found in order");

        Items.Remove(item);
        RecalculateTotals();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateItemQuantity(Guid itemId, int quantity)
    {
        if (quantity <= 0)
            throw new InvalidOperationException("Quantity must be greater than zero");

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            throw new InvalidOperationException("Item not found in order");

        item.Quantity = quantity;
        item.Total = item.Price * quantity;
        RecalculateTotals();
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecalculateTotals()
    {
        Subtotal = Items.Sum(i => i.Total);
        Tax = Subtotal * 0.10m; // 10% tax
        Total = Subtotal + Tax;
    }

    public void UpdateStatus(string status)
    {
        var validStatuses = new[] { "pending", "processing", "completed", "cancelled" };
        if (!validStatuses.Contains(status.ToLower()))
            throw new InvalidOperationException($"Invalid status: {status}");

        Status = status.ToLower();
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPaymentMethod(string paymentMethod)
    {
        if (string.IsNullOrWhiteSpace(paymentMethod))
            throw new ArgumentException("Payment method cannot be empty", nameof(paymentMethod));

        PaymentMethod = paymentMethod;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        Status = "completed";
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = "cancelled";
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsCompleted()
    {
        return Status == "completed";
    }

    public bool IsCancelled()
    {
        return Status == "cancelled";
    }
}

