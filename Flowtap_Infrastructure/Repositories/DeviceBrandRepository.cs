using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.BoundedContexts.Service.Interfaces;
using Flowtap_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flowtap_Infrastructure.Repositories;

public class DeviceBrandRepository : IDeviceBrandRepository
{
    private readonly AppDbContext _context;

    public DeviceBrandRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DeviceBrand?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DeviceBrands
            .Include(db => db.Category)
            .Include(db => db.Models)
            .FirstOrDefaultAsync(db => db.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<DeviceBrand>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DeviceBrands
            .Include(db => db.Category)
            .OrderBy(db => db.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DeviceBrand>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _context.DeviceBrands
            .Include(db => db.Category)
            .Where(db => db.CategoryId == categoryId)
            .OrderBy(db => db.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<DeviceBrand> CreateAsync(DeviceBrand brand, CancellationToken cancellationToken = default)
    {
        brand.Id = Guid.NewGuid();
        _context.DeviceBrands.Add(brand);
        await _context.SaveChangesAsync(cancellationToken);
        return brand;
    }

    public async Task UpdateAsync(DeviceBrand brand, CancellationToken cancellationToken = default)
    {
        _context.DeviceBrands.Update(brand);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var brand = await _context.DeviceBrands.FindAsync(new object[] { id }, cancellationToken);
        if (brand == null)
            return false;

        _context.DeviceBrands.Remove(brand);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

