using Flowtap_Domain.BoundedContexts.Inventory.Entities;
using Flowtap_Domain.BoundedContexts.Inventory.Interfaces;
using Flowtap_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flowtap_Infrastructure.Repositories;

public class StockTransferRepository : IStockTransferRepository
{
    private readonly AppDbContext _context;

    public StockTransferRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StockTransfer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.StockTransfers
            .Include(st => st.Items)
            .ThenInclude(item => item.InventoryItem)
            .FirstOrDefaultAsync(st => st.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<StockTransfer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.StockTransfers
            .Include(st => st.Items)
            .OrderByDescending(st => st.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StockTransfer>> GetByFromStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default)
    {
        return await _context.StockTransfers
            .Include(st => st.Items)
            .Where(st => st.FromStoreId == storeId)
            .OrderByDescending(st => st.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StockTransfer>> GetByToStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default)
    {
        return await _context.StockTransfers
            .Include(st => st.Items)
            .Where(st => st.ToStoreId == storeId)
            .OrderByDescending(st => st.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StockTransfer>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.StockTransfers
            .Include(st => st.Items)
            .Where(st => st.Status == status)
            .OrderByDescending(st => st.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<StockTransfer> CreateAsync(StockTransfer transfer, CancellationToken cancellationToken = default)
    {
        transfer.Id = Guid.NewGuid();
        transfer.CreatedAt = DateTime.UtcNow;
        _context.StockTransfers.Add(transfer);
        await _context.SaveChangesAsync(cancellationToken);
        return transfer;
    }

    public async Task UpdateAsync(StockTransfer transfer, CancellationToken cancellationToken = default)
    {
        _context.StockTransfers.Update(transfer);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transfer = await _context.StockTransfers.FindAsync(new object[] { id }, cancellationToken);
        if (transfer == null)
            return false;

        _context.StockTransfers.Remove(transfer);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

