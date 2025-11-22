using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flowtap_Domain.SharedKernel.Enums;

namespace Flowtap_Domain.BoundedContexts.Sales.Entities;

public class Invoice
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid StoreId { get; set; }

    [Required]
    public Guid CustomerId { get; set; }

    public Customer? Customer { get; set; }

    [MaxLength(100)]
    public string InvoiceNumber { get; set; } = string.Empty;

    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DueDate { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Tax { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Adds an item to the invoice
    /// </summary>
    public void AddItem(InvoiceItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (Status != InvoiceStatus.Draft)
            throw new InvalidOperationException("Cannot add items to an invoice that is not in Draft status");

        Items.Add(item);
        RecalculateTotals();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes an item from the invoice
    /// </summary>
    public void RemoveItem(Guid itemId)
    {
        if (Status != InvoiceStatus.Draft)
            throw new InvalidOperationException("Cannot remove items from an invoice that is not in Draft status");

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            throw new InvalidOperationException("Item not found in invoice");

        Items.Remove(item);
        RecalculateTotals();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Recalculates the invoice totals
    /// </summary>
    public void RecalculateTotals()
    {
        Subtotal = Items.Sum(i => i.Total);
        // Tax calculation can be customized based on business rules
        Total = Subtotal + Tax;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the tax amount
    /// </summary>
    public void UpdateTax(decimal tax)
    {
        if (tax < 0)
            throw new ArgumentException("Tax cannot be negative", nameof(tax));

        Tax = tax;
        RecalculateTotals();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the invoice status
    /// </summary>
    public void UpdateStatus(InvoiceStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;

        if (status == InvoiceStatus.Paid)
        {
            PaidAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Marks the invoice as sent
    /// </summary>
    public void MarkAsSent()
    {
        if (Status != InvoiceStatus.Draft)
            throw new InvalidOperationException("Only draft invoices can be marked as sent");

        Status = InvoiceStatus.Sent;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the invoice as paid
    /// </summary>
    public void MarkAsPaid()
    {
        if (Status == InvoiceStatus.Paid)
            throw new InvalidOperationException("Invoice is already marked as paid");

        Status = InvoiceStatus.Paid;
        PaidAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the invoice as overdue
    /// </summary>
    public void MarkAsOverdue()
    {
        if (Status == InvoiceStatus.Paid || Status == InvoiceStatus.Cancelled)
            throw new InvalidOperationException("Cannot mark paid or cancelled invoices as overdue");

        if (!DueDate.HasValue)
            throw new InvalidOperationException("Invoice must have a due date to be marked as overdue");

        if (DueDate.Value > DateTime.UtcNow)
            throw new InvalidOperationException("Invoice due date has not passed yet");

        Status = InvoiceStatus.Overdue;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels the invoice
    /// </summary>
    public void Cancel()
    {
        if (Status == InvoiceStatus.Paid)
            throw new InvalidOperationException("Cannot cancel a paid invoice");

        Status = InvoiceStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the due date
    /// </summary>
    public void SetDueDate(DateTime dueDate)
    {
        if (dueDate < CreatedAt)
            throw new InvalidOperationException("Due date cannot be before creation date");

        DueDate = dueDate;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the total amount paid
    /// </summary>
    public decimal GetTotalPaid()
    {
        return Payments.Sum(p => p.Amount);
    }

    /// <summary>
    /// Gets the remaining balance
    /// </summary>
    public decimal GetBalance()
    {
        return Total - GetTotalPaid();
    }

    /// <summary>
    /// Checks if the invoice is fully paid
    /// </summary>
    public bool IsFullyPaid()
    {
        return GetTotalPaid() >= Total;
    }

    /// <summary>
    /// Checks if the invoice is overdue
    /// </summary>
    public bool IsOverdue()
    {
        return DueDate.HasValue && 
               DueDate.Value < DateTime.UtcNow && 
               Status != InvoiceStatus.Paid && 
               Status != InvoiceStatus.Cancelled;
    }
}

