using StoreEntity = Flowtap_Domain.BoundedContexts.Store.Entities.Store;

namespace Flowtap_Domain.BoundedContexts.Store.Interfaces;

public interface IStoreRepository
{
    Task<StoreEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<StoreEntity>> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default);
    Task<IEnumerable<StoreEntity>> GetByStoreTypeAsync(string storeType, CancellationToken cancellationToken = default);
    Task<IEnumerable<StoreEntity>> GetByStoreCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<StoreEntity> AddAsync(StoreEntity store, CancellationToken cancellationToken = default);
    Task UpdateAsync(StoreEntity store, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> GetStoreCountByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default);
}

