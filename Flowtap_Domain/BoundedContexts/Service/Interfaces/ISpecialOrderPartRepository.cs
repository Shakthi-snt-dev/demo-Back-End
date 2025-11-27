using Flowtap_Domain.BoundedContexts.Service.Entities;

namespace Flowtap_Domain.BoundedContexts.Service.Interfaces;

public interface ISpecialOrderPartRepository
{
    Task<SpecialOrderPart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SpecialOrderPart>> GetByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SpecialOrderPart>> GetBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SpecialOrderPart>> GetPendingOrdersAsync(Guid storeId, CancellationToken cancellationToken = default);
    Task<SpecialOrderPart> CreateAsync(SpecialOrderPart part, CancellationToken cancellationToken = default);
    Task UpdateAsync(SpecialOrderPart part, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

