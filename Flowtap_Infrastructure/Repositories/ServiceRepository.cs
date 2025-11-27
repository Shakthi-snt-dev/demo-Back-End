using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.BoundedContexts.Service.Interfaces;
using Flowtap_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ServiceEntity = Flowtap_Domain.BoundedContexts.Service.Entities.Service;

namespace Flowtap_Infrastructure.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly AppDbContext _context;

    public ServiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .Include(s => s.Parts)
            .Include(s => s.Labor)
            .Include(s => s.Warranties)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ServiceEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .Include(s => s.Parts)
            .Include(s => s.Labor)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ServiceEntity>> GetByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .Include(s => s.Parts)
            .Include(s => s.Labor)
            .Where(s => s.StoreId == storeId)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ServiceEntity>> GetActiveServicesAsync(Guid storeId, CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .Include(s => s.Parts)
            .Include(s => s.Labor)
            .Where(s => s.StoreId == storeId && s.IsActive)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<ServiceEntity> CreateAsync(ServiceEntity service, CancellationToken cancellationToken = default)
    {
        service.Id = Guid.NewGuid();
        service.CreatedAt = DateTime.UtcNow;
        service.UpdatedAt = DateTime.UtcNow;
        _context.Services.Add(service);
        await _context.SaveChangesAsync(cancellationToken);
        return service;
    }

    public async Task UpdateAsync(ServiceEntity service, CancellationToken cancellationToken = default)
    {
        service.UpdatedAt = DateTime.UtcNow;
        _context.Services.Update(service);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var service = await _context.Services.FindAsync(new object[] { id }, cancellationToken);
        if (service == null)
            return false;

        _context.Services.Remove(service);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

