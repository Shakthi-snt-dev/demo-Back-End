using Microsoft.EntityFrameworkCore;
using StoreEntity = Flowtap_Domain.BoundedContexts.Store.Entities.Store;
using Flowtap_Domain.BoundedContexts.Store.Interfaces;
using Flowtap_Infrastructure.Data;

namespace Flowtap_Infrastructure.Repositories;

public class StoreRepository : IStoreRepository
{
    private readonly AppDbContext _context;

    public StoreRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StoreEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Stores
            .Include(s => s.Settings)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<StoreEntity>> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default)
    {
        return await _context.Stores
            .Include(s => s.Settings)
            .Where(s => s.AppUserId == appUserId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StoreEntity>> GetByStoreTypeAsync(string storeType, CancellationToken cancellationToken = default)
    {
        return await _context.Stores
            .Include(s => s.Settings)
            .Where(s => s.StoreType == storeType)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StoreEntity>> GetByStoreCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        return await _context.Stores
            .Include(s => s.Settings)
            .Where(s => s.StoreCategory == category)
            .ToListAsync(cancellationToken);
    }

    public async Task<StoreEntity> AddAsync(StoreEntity store, CancellationToken cancellationToken = default)
    {
        await _context.Stores.AddAsync(store, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return store;
    }

    public async Task UpdateAsync(StoreEntity store, CancellationToken cancellationToken = default)
    {
        _context.Stores.Update(store);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var store = await GetByIdAsync(id, cancellationToken);
        if (store != null)
        {
            _context.Stores.Remove(store);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Stores
            .AnyAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<int> GetStoreCountByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default)
    {
        return await _context.Stores
            .CountAsync(s => s.AppUserId == appUserId, cancellationToken);
    }
}

