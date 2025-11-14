using Flowtap_Domain.BoundedContexts.Store.Entities;

namespace Flowtap_Domain.BoundedContexts.Store.Interfaces;

public interface IStoreRepository
{
    Task<Store?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Store>> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Store>> GetByStoreTypeAsync(string storeType, CancellationToken cancellationToken = default);
    Task<IEnumerable<Store>> GetByStoreCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<Store> AddAsync(Store store, CancellationToken cancellationToken = default);
    Task UpdateAsync(Store store, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> GetStoreCountByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default);
}

