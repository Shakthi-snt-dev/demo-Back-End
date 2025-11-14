using Microsoft.EntityFrameworkCore;
using Flowtap_Domain.BoundedContexts.Store.Entities;
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

    public async Task<Store?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Stores
            .Include(s => s.Settings)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Store>> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default)
    {
        return await _context.Stores
            .Include(s => s.Settings)
            .Where(s => s.AppUserId == appUserId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Store>> GetByStoreTypeAsync(string storeType, CancellationToken cancellationToken = default)
    {
        return await _context.Stores
            .Include(s => s.Settings)
            .Where(s => s.StoreType == storeType)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Store>> GetByStoreCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        return await _context.Stores
            .Include(s => s.Settings)
            .Where(s => s.StoreCategory == category)
            .ToListAsync(cancellationToken);
    }

    public async Task<Store> AddAsync(Store store, CancellationToken cancellationToken = default)
    {
        await _context.Stores.AddAsync(store, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return store;
    }

    public async Task UpdateAsync(Store store, CancellationToken cancellationToken = default)
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

