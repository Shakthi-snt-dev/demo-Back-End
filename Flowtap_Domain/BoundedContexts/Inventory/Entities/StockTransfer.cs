using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Inventory.Entities;

public class StockTransfer
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid FromStoreId { get; set; }
    // Note: Store is in Store context - reference by ID only, no navigation property

    [Required]
    public Guid ToStoreId { get; set; }
    // Note: Store is in Store context - reference by ID only, no navigation property

    public string Status { get; set; } = "Pending"; // Pending, Approved, InTransit, Completed

    public Guid? RequestedById { get; set; }
    // Note: Employee is in HR context - reference by ID only, no navigation property

    public Guid? ApprovedById { get; set; }
    // Note: Employee is in HR context - reference by ID only, no navigation property

    public ICollection<StockTransferItem> Items { get; set; } = new List<StockTransferItem>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Adds an item to the stock transfer
    /// </summary>
    public void AddItem(Guid inventoryItemId, int quantity)
    {
        if (Status != "Pending")
            throw new InvalidOperationException("Cannot add items to a transfer that is not pending");

        var item = StockTransferItem.Create(Id, inventoryItemId, quantity);
        Items.Add(item);
    }

    /// <summary>
    /// Approves the stock transfer
    /// </summary>
    public void Approve(Guid approvedById)
    {
        if (Status != "Pending")
            throw new InvalidOperationException("Only pending transfers can be approved");
        if (Items.Count == 0)
            throw new InvalidOperationException("Cannot approve a transfer without items");
        if (FromStoreId == ToStoreId)
            throw new InvalidOperationException("Cannot transfer to the same store");

        Status = "Approved";
        ApprovedById = approvedById;
    }

    /// <summary>
    /// Marks the transfer as in transit
    /// </summary>
    public void MarkAsInTransit()
    {
        if (Status != "Approved")
            throw new InvalidOperationException("Only approved transfers can be marked as in transit");

        Status = "InTransit";
    }

    /// <summary>
    /// Marks the transfer as completed
    /// </summary>
    public void MarkAsCompleted()
    {
        if (Status != "InTransit" && Status != "Approved")
            throw new InvalidOperationException("Only in-transit or approved transfers can be marked as completed");

        Status = "Completed";
    }

    /// <summary>
    /// Gets the total quantity of items being transferred
    /// </summary>
    public int GetTotalQuantity()
    {
        return Items.Sum(item => item.Quantity);
    }
}

