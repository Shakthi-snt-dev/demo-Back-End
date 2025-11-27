using Flowtap_Domain.BoundedContexts.Inventory.Entities;

namespace Flowtap_Domain.BoundedContexts.Inventory.Interfaces;

public interface ISerialNumberRepository
{
    Task<SerialNumber?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SerialNumber>> GetByInventoryItemIdAsync(Guid inventoryItemId, CancellationToken cancellationToken = default);
    Task<SerialNumber?> GetBySerialAsync(string serial, CancellationToken cancellationToken = default);
    Task<IEnumerable<SerialNumber>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<SerialNumber> CreateAsync(SerialNumber serialNumber, CancellationToken cancellationToken = default);
    Task UpdateAsync(SerialNumber serialNumber, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}

