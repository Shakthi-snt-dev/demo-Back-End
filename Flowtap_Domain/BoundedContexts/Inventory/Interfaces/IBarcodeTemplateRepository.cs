using Flowtap_Domain.BoundedContexts.Inventory.Entities;

namespace Flowtap_Domain.BoundedContexts.Inventory.Interfaces;

public interface IBarcodeTemplateRepository
{
    Task<BarcodeTemplate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BarcodeTemplate>> GetByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default);
    Task<BarcodeTemplate?> GetDefaultByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default);
    Task<BarcodeTemplate> CreateAsync(BarcodeTemplate template, CancellationToken cancellationToken = default);
    Task UpdateAsync(BarcodeTemplate template, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

