using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.BoundedContexts.Service.Interfaces;
using Flowtap_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flowtap_Infrastructure.Repositories;

public class DeviceModelRepository : IDeviceModelRepository
{
    private readonly AppDbContext _context;

    public DeviceModelRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DeviceModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DeviceModels
            .Include(dm => dm.Brand)
            .ThenInclude(db => db.Category)
            .Include(dm => dm.Variants)
            .Include(dm => dm.Problems)
            .FirstOrDefaultAsync(dm => dm.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<DeviceModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DeviceModels
            .Include(dm => dm.Brand)
            .OrderBy(dm => dm.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DeviceModel>> GetByBrandIdAsync(Guid brandId, CancellationToken cancellationToken = default)
    {
        return await _context.DeviceModels
            .Include(dm => dm.Brand)
            .Where(dm => dm.BrandId == brandId)
            .OrderBy(dm => dm.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<DeviceModel> CreateAsync(DeviceModel model, CancellationToken cancellationToken = default)
    {
        model.Id = Guid.NewGuid();
        _context.DeviceModels.Add(model);
        await _context.SaveChangesAsync(cancellationToken);
        return model;
    }

    public async Task UpdateAsync(DeviceModel model, CancellationToken cancellationToken = default)
    {
        _context.DeviceModels.Update(model);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var model = await _context.DeviceModels.FindAsync(new object[] { id }, cancellationToken);
        if (model == null)
            return false;

        _context.DeviceModels.Remove(model);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

