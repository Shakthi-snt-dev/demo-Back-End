using Flowtap_Domain.BoundedContexts.Inventory.Entities;

namespace Flowtap_Domain.BoundedContexts.Inventory.Interfaces;

public interface IInventoryTransactionRepository
{
    Task<InventoryTransaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryTransaction>> GetByInventoryItemIdAsync(Guid inventoryItemId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryTransaction>> GetByTypeAsync(SharedKernel.Enums.InventoryTransactionType type, CancellationToken cancellationToken = default);
    Task<InventoryTransaction> CreateAsync(InventoryTransaction transaction, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

