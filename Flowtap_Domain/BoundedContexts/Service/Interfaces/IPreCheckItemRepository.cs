using Flowtap_Domain.BoundedContexts.Service.Entities;

namespace Flowtap_Domain.BoundedContexts.Service.Interfaces;

public interface IPreCheckItemRepository
{
    Task<PreCheckItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PreCheckItem>> GetByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PreCheckItem>> GetActiveItemsAsync(Guid storeId, CancellationToken cancellationToken = default);
    Task<PreCheckItem> CreateAsync(PreCheckItem item, CancellationToken cancellationToken = default);
    Task UpdateAsync(PreCheckItem item, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

