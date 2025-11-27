using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.BoundedContexts.Service.Interfaces;
using Flowtap_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flowtap_Infrastructure.Repositories;

public class DeviceVariantRepository : IDeviceVariantRepository
{
    private readonly AppDbContext _context;

    public DeviceVariantRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DeviceVariant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DeviceVariants
            .Include(dv => dv.Model)
            .ThenInclude(dm => dm.Brand)
            .FirstOrDefaultAsync(dv => dv.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<DeviceVariant>> GetByModelIdAsync(Guid modelId, CancellationToken cancellationToken = default)
    {
        return await _context.DeviceVariants
            .Where(dv => dv.ModelId == modelId)
            .OrderBy(dv => dv.Attribute)
            .ThenBy(dv => dv.Value)
            .ToListAsync(cancellationToken);
    }

    public async Task<DeviceVariant> CreateAsync(DeviceVariant variant, CancellationToken cancellationToken = default)
    {
        variant.Id = Guid.NewGuid();
        _context.DeviceVariants.Add(variant);
        await _context.SaveChangesAsync(cancellationToken);
        return variant;
    }

    public async Task UpdateAsync(DeviceVariant variant, CancellationToken cancellationToken = default)
    {
        _context.DeviceVariants.Update(variant);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var variant = await _context.DeviceVariants.FindAsync(new object[] { id }, cancellationToken);
        if (variant == null)
            return false;

        _context.DeviceVariants.Remove(variant);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

