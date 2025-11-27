using Flowtap_Domain.BoundedContexts.Inventory.Entities;

namespace Flowtap_Domain.BoundedContexts.Inventory.Interfaces;

public interface IInventorySettingsRepository
{
    Task<InventorySettings?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<InventorySettings?> GetByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default);
    Task<InventorySettings> CreateAsync(InventorySettings settings, CancellationToken cancellationToken = default);
    Task UpdateAsync(InventorySettings settings, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}

