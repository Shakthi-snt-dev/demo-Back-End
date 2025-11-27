using Flowtap_Domain.BoundedContexts.Inventory.Entities;

namespace Flowtap_Domain.BoundedContexts.Inventory.Interfaces;

public interface IInventoryItemRepository
{
    Task<InventoryItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryItem>> GetByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryItem>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<InventoryItem?> GetByProductIdAndStoreIdAsync(Guid productId, Guid storeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryItem>> GetLowStockItemsAsync(Guid storeId, CancellationToken cancellationToken = default);
    Task<InventoryItem> CreateAsync(InventoryItem inventoryItem, CancellationToken cancellationToken = default);
    Task UpdateAsync(InventoryItem inventoryItem, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}

