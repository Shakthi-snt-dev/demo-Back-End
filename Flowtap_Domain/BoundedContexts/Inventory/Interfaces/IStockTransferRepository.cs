using Flowtap_Domain.BoundedContexts.Inventory.Entities;

namespace Flowtap_Domain.BoundedContexts.Inventory.Interfaces;

public interface IStockTransferRepository
{
    Task<StockTransfer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<StockTransfer>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<StockTransfer>> GetByFromStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<StockTransfer>> GetByToStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<StockTransfer>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<StockTransfer> CreateAsync(StockTransfer transfer, CancellationToken cancellationToken = default);
    Task UpdateAsync(StockTransfer transfer, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

