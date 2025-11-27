using Flowtap_Domain.BoundedContexts.Procurement.Entities;

namespace Flowtap_Domain.BoundedContexts.Procurement.Interfaces;

public interface ISupplierReturnRepository
{
    Task<SupplierReturn?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SupplierReturn>> GetByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SupplierReturn>> GetBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SupplierReturn>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<SupplierReturn> CreateAsync(SupplierReturn supplierReturn, CancellationToken cancellationToken = default);
    Task UpdateAsync(SupplierReturn supplierReturn, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

