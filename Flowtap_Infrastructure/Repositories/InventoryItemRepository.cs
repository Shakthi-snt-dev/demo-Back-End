using Flowtap_Domain.BoundedContexts.Inventory.Entities;
using Flowtap_Domain.BoundedContexts.Inventory.Interfaces;
using Flowtap_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flowtap_Infrastructure.Repositories;

public class InventoryItemRepository : IInventoryItemRepository
{
    private readonly AppDbContext _context;

    public InventoryItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .Include(i => i.Product)
            .Include(i => i.Serials)
            .Include(i => i.Transactions)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<InventoryItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .Include(i => i.Product)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InventoryItem>> GetByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .Include(i => i.Product)
            .Where(i => i.StoreId == storeId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InventoryItem>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .Include(i => i.Product)
            .Where(i => i.ProductId == productId)
            .ToListAsync(cancellationToken);
    }

    public async Task<InventoryItem?> GetByProductIdAndStoreIdAsync(Guid productId, Guid storeId, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .Include(i => i.Product)
            .FirstOrDefaultAsync(i => i.ProductId == productId && i.StoreId == storeId, cancellationToken);
    }

    public async Task<IEnumerable<InventoryItem>> GetLowStockItemsAsync(Guid storeId, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .Include(i => i.Product)
            .Where(i => i.StoreId == storeId && i.QuantityOnHand <= i.ReorderLevel)
            .ToListAsync(cancellationToken);
    }

    public async Task<InventoryItem> CreateAsync(InventoryItem inventoryItem, CancellationToken cancellationToken = default)
    {
        inventoryItem.Id = Guid.NewGuid();
        inventoryItem.UpdatedAt = DateTime.UtcNow;
        _context.InventoryItems.Add(inventoryItem);
        await _context.SaveChangesAsync(cancellationToken);
        return inventoryItem;
    }

    public async Task UpdateAsync(InventoryItem inventoryItem, CancellationToken cancellationToken = default)
    {
        inventoryItem.UpdatedAt = DateTime.UtcNow;
        _context.InventoryItems.Update(inventoryItem);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _context.InventoryItems.FindAsync(new object[] { id }, cancellationToken);
        if (item == null)
            return false;

        _context.InventoryItems.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems.AnyAsync(i => i.Id == id, cancellationToken);
    }
}

