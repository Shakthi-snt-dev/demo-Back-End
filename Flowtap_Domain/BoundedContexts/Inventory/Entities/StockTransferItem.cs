using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.BoundedContexts.Inventory.Entities;

public class StockTransferItem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid StockTransferId { get; set; }

    public StockTransfer? StockTransfer { get; set; }

    public Guid InventoryItemId { get; set; }

    public InventoryItem? InventoryItem { get; set; }

    public int Quantity { get; set; }

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Creates a new stock transfer item
    /// </summary>
    public static StockTransferItem Create(Guid stockTransferId, Guid inventoryItemId, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        return new StockTransferItem
        {
            Id = Guid.NewGuid(),
            StockTransferId = stockTransferId,
            InventoryItemId = inventoryItemId,
            Quantity = quantity
        };
    }

    /// <summary>
    /// Updates the quantity
    /// </summary>
    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        Quantity = quantity;
    }
}

