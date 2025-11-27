using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Procurement.Entities;

public class SupplierReturn
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid SupplierId { get; set; }

    public Supplier? Supplier { get; set; }

    public Guid StoreId { get; set; }
    // Note: Store is in Store context - reference by ID only, no navigation property

    public string Status { get; set; } = "Pending"; // Pending, Shipped, Completed

    public ICollection<SupplierReturnItem> Items { get; set; } = new List<SupplierReturnItem>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Adds an item to the supplier return
    /// </summary>
    public void AddItem(Guid inventoryItemId, int quantity)
    {
        if (Status != "Pending")
            throw new InvalidOperationException("Cannot add items to a return that is not pending");

        var item = SupplierReturnItem.Create(Id, inventoryItemId, quantity);
        Items.Add(item);
    }

    /// <summary>
    /// Marks the return as shipped
    /// </summary>
    public void MarkAsShipped()
    {
        if (Status != "Pending")
            throw new InvalidOperationException("Only pending returns can be marked as shipped");
        if (Items.Count == 0)
            throw new InvalidOperationException("Cannot ship a return without items");

        Status = "Shipped";
    }

    /// <summary>
    /// Marks the return as completed
    /// </summary>
    public void MarkAsCompleted()
    {
        if (Status != "Shipped")
            throw new InvalidOperationException("Only shipped returns can be marked as completed");

        Status = "Completed";
    }

    /// <summary>
    /// Gets the total quantity of items being returned
    /// </summary>
    public int GetTotalQuantity()
    {
        return Items.Sum(item => item.Quantity);
    }
}

