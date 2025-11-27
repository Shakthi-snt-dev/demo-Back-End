using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.BoundedContexts.Service.Interfaces;
using Flowtap_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flowtap_Infrastructure.Repositories;

public class PreCheckItemRepository : IPreCheckItemRepository
{
    private readonly AppDbContext _context;

    public PreCheckItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PreCheckItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.PreCheckItems.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<PreCheckItem>> GetByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default)
    {
        return await _context.PreCheckItems
            .Where(pci => pci.StoreId == storeId)
            .OrderBy(pci => pci.DisplayOrder)
            .ThenBy(pci => pci.Description)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PreCheckItem>> GetActiveItemsAsync(Guid storeId, CancellationToken cancellationToken = default)
    {
        return await _context.PreCheckItems
            .Where(pci => pci.StoreId == storeId && pci.IsActive)
            .OrderBy(pci => pci.DisplayOrder)
            .ThenBy(pci => pci.Description)
            .ToListAsync(cancellationToken);
    }

    public async Task<PreCheckItem> CreateAsync(PreCheckItem item, CancellationToken cancellationToken = default)
    {
        item.Id = Guid.NewGuid();
        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;
        _context.PreCheckItems.Add(item);
        await _context.SaveChangesAsync(cancellationToken);
        return item;
    }

    public async Task UpdateAsync(PreCheckItem item, CancellationToken cancellationToken = default)
    {
        item.UpdatedAt = DateTime.UtcNow;
        _context.PreCheckItems.Update(item);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _context.PreCheckItems.FindAsync(new object[] { id }, cancellationToken);
        if (item == null)
            return false;

        _context.PreCheckItems.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

