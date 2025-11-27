using System.ComponentModel.DataAnnotations;
using Flowtap_Domain.SharedKernel.Enums;

namespace Flowtap_Domain.BoundedContexts.Procurement.Entities;

public class PurchaseOrder
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(100)]
    public string? PONumber { get; set; }

    [Required]
    public Guid SupplierId { get; set; }

    public Supplier? Supplier { get; set; }

    [Required]
    public Guid StoreId { get; set; }
    // Note: Store is in Store context - reference by ID only, no navigation property

    public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Open;

    public ICollection<PurchaseOrderLine> Lines { get; set; } = new List<PurchaseOrderLine>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Adds a line item to the purchase order
    /// </summary>
    public void AddLine(Guid productId, int quantity, decimal unitCost)
    {
        if (Status != PurchaseOrderStatus.Open)
            throw new InvalidOperationException("Cannot add lines to a purchase order that is not open");

        var line = PurchaseOrderLine.Create(Id, productId, quantity, unitCost);
        Lines.Add(line);
    }

    /// <summary>
    /// Submits the purchase order
    /// </summary>
    public void Submit()
    {
        if (Status != PurchaseOrderStatus.Open)
            throw new InvalidOperationException("Only open purchase orders can be submitted");
        if (Lines.Count == 0)
            throw new InvalidOperationException("Cannot submit a purchase order without line items");

        Status = PurchaseOrderStatus.Submitted;
    }

    /// <summary>
    /// Marks the purchase order as received
    /// </summary>
    public void MarkAsReceived()
    {
        if (Status == PurchaseOrderStatus.Cancelled)
            throw new InvalidOperationException("Cannot mark a cancelled purchase order as received");

        Status = PurchaseOrderStatus.Received;
    }

    /// <summary>
    /// Marks the purchase order as partially received
    /// </summary>
    public void MarkAsPartiallyReceived()
    {
        if (Status == PurchaseOrderStatus.Cancelled)
            throw new InvalidOperationException("Cannot mark a cancelled purchase order as partially received");

        Status = PurchaseOrderStatus.PartiallyReceived;
    }

    /// <summary>
    /// Cancels the purchase order
    /// </summary>
    public void Cancel()
    {
        if (Status == PurchaseOrderStatus.Received)
            throw new InvalidOperationException("Cannot cancel a received purchase order");

        Status = PurchaseOrderStatus.Cancelled;
    }

    /// <summary>
    /// Calculates the total amount of the purchase order
    /// </summary>
    public decimal GetTotalAmount()
    {
        return Lines.Sum(line => line.GetTotalCost());
    }

    /// <summary>
    /// Generates a PO number if not set
    /// </summary>
    public void GeneratePONumber()
    {
        if (string.IsNullOrWhiteSpace(PONumber))
        {
            PONumber = $"PO-{CreatedAt:yyyyMMdd}-{Id.ToString().Substring(0, 8).ToUpper()}";
        }
    }
}

