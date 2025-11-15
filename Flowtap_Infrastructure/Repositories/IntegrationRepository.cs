using IntegrationEntity = Flowtap_Domain.BoundedContexts.Integration.Entities.Integration;
using Flowtap_Domain.BoundedContexts.Integration.Interfaces;
using Flowtap_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flowtap_Infrastructure.Repositories;

public class IntegrationRepository : IIntegrationRepository
{
    private readonly AppDbContext _context;

    public IntegrationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IntegrationEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Integrations.FindAsync(id);
    }

    public async Task<IEnumerable<IntegrationEntity>> GetByAppUserIdAsync(Guid appUserId)
    {
        return await _context.Integrations
            .Where(i => i.AppUserId == appUserId)
            .ToListAsync();
    }

    public async Task<IntegrationEntity?> GetByAppUserIdAndTypeAsync(Guid appUserId, string type)
    {
        return await _context.Integrations
            .FirstOrDefaultAsync(i => i.AppUserId == appUserId && i.Type == type);
    }

    public async Task<IEnumerable<IntegrationEntity>> GetConnectedIntegrationsAsync(Guid appUserId)
    {
        return await _context.Integrations
            .Where(i => i.AppUserId == appUserId && i.Connected)
            .ToListAsync();
    }

    public async Task<IntegrationEntity> CreateAsync(IntegrationEntity integration)
    {
        integration.Id = Guid.NewGuid();
        integration.CreatedAt = DateTime.UtcNow;
        integration.UpdatedAt = DateTime.UtcNow;
        _context.Integrations.Add(integration);
        await _context.SaveChangesAsync();
        return integration;
    }

    public async Task<IntegrationEntity> UpdateAsync(IntegrationEntity integration)
    {
        integration.UpdatedAt = DateTime.UtcNow;
        _context.Integrations.Update(integration);
        await _context.SaveChangesAsync();
        return integration;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var integration = await _context.Integrations.FindAsync(id);
        if (integration == null)
            return false;

        _context.Integrations.Remove(integration);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Integrations.AnyAsync(i => i.Id == id);
    }
}

