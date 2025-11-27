using Flowtap_Domain.BoundedContexts.Inventory.Entities;

namespace Flowtap_Domain.BoundedContexts.Inventory.Interfaces;

public interface IInventoryAdjustmentRepository
{
    Task<InventoryAdjustment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryAdjustment>> GetByInventoryItemIdAsync(Guid inventoryItemId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryAdjustment>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<InventoryAdjustment> CreateAsync(InventoryAdjustment adjustment, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

