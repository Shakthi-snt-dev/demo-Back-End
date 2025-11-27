using Flowtap_Domain.BoundedContexts.Inventory.Entities;
using Flowtap_Domain.BoundedContexts.Inventory.Interfaces;
using Flowtap_Domain.SharedKernel.Enums;
using Flowtap_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flowtap_Infrastructure.Repositories;

public class InventoryTransactionRepository : IInventoryTransactionRepository
{
    private readonly AppDbContext _context;

    public InventoryTransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryTransaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryTransactions
            .Include(t => t.InventoryItem)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<InventoryTransaction>> GetByInventoryItemIdAsync(Guid inventoryItemId, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryTransactions
            .Where(t => t.InventoryItemId == inventoryItemId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InventoryTransaction>> GetByTypeAsync(InventoryTransactionType type, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryTransactions
            .Include(t => t.InventoryItem)
            .Where(t => t.Type == type)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<InventoryTransaction> CreateAsync(InventoryTransaction transaction, CancellationToken cancellationToken = default)
    {
        transaction.Id = Guid.NewGuid();
        transaction.CreatedAt = DateTime.UtcNow;
        _context.InventoryTransactions.Add(transaction);
        await _context.SaveChangesAsync(cancellationToken);
        return transaction;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await _context.InventoryTransactions.FindAsync(new object[] { id }, cancellationToken);
        if (transaction == null)
            return false;

        _context.InventoryTransactions.Remove(transaction);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

