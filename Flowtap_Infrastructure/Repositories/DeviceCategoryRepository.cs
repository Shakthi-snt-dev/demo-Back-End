using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.BoundedContexts.Service.Interfaces;
using Flowtap_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flowtap_Infrastructure.Repositories;

public class DeviceCategoryRepository : IDeviceCategoryRepository
{
    private readonly AppDbContext _context;

    public DeviceCategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DeviceCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DeviceCategories
            .Include(dc => dc.Brands)
            .ThenInclude(db => db.Models)
            .FirstOrDefaultAsync(dc => dc.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<DeviceCategory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DeviceCategories
            .Include(dc => dc.Brands)
            .OrderBy(dc => dc.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<DeviceCategory?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.DeviceCategories
            .Include(dc => dc.Brands)
            .FirstOrDefaultAsync(dc => dc.Name == name, cancellationToken);
    }

    public async Task<DeviceCategory> CreateAsync(DeviceCategory category, CancellationToken cancellationToken = default)
    {
        category.Id = Guid.NewGuid();
        _context.DeviceCategories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);
        return category;
    }

    public async Task UpdateAsync(DeviceCategory category, CancellationToken cancellationToken = default)
    {
        _context.DeviceCategories.Update(category);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _context.DeviceCategories.FindAsync(new object[] { id }, cancellationToken);
        if (category == null)
            return false;

        _context.DeviceCategories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

