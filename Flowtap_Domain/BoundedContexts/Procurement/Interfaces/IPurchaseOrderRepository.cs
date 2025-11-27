using Flowtap_Domain.BoundedContexts.Procurement.Entities;

namespace Flowtap_Domain.BoundedContexts.Procurement.Interfaces;

public interface IPurchaseOrderRepository
{
    Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PurchaseOrder>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<PurchaseOrder>> GetByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PurchaseOrder>> GetBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken = default);
    Task<PurchaseOrder?> GetByPONumberAsync(string poNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<PurchaseOrder>> GetByStatusAsync(SharedKernel.Enums.PurchaseOrderStatus status, CancellationToken cancellationToken = default);
    Task<PurchaseOrder> CreateAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default);
    Task UpdateAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

