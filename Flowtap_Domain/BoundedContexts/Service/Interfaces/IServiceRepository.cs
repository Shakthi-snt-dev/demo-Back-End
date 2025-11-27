using Flowtap_Domain.BoundedContexts.Service.Entities;
using ServiceEntity = Flowtap_Domain.BoundedContexts.Service.Entities.Service;

namespace Flowtap_Domain.BoundedContexts.Service.Interfaces;

public interface IServiceRepository
{
    Task<ServiceEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ServiceEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ServiceEntity>> GetByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ServiceEntity>> GetActiveServicesAsync(Guid storeId, CancellationToken cancellationToken = default);
    Task<ServiceEntity> CreateAsync(ServiceEntity service, CancellationToken cancellationToken = default);
    Task UpdateAsync(ServiceEntity service, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

