using Flowtap_Domain.BoundedContexts.Integration.Entities;
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

    public async Task<Integration?> GetByIdAsync(Guid id)
    {
        return await _context.Integrations.FindAsync(id);
    }

    public async Task<IEnumerable<Integration>> GetByAppUserIdAsync(Guid appUserId)
    {
        return await _context.Integrations
            .Where(i => i.AppUserId == appUserId)
            .ToListAsync();
    }

    public async Task<Integration?> GetByAppUserIdAndTypeAsync(Guid appUserId, string type)
    {
        return await _context.Integrations
            .FirstOrDefaultAsync(i => i.AppUserId == appUserId && i.Type == type);
    }

    public async Task<IEnumerable<Integration>> GetConnectedIntegrationsAsync(Guid appUserId)
    {
        return await _context.Integrations
            .Where(i => i.AppUserId == appUserId && i.Connected)
            .ToListAsync();
    }

    public async Task<Integration> CreateAsync(Integration integration)
    {
        integration.Id = Guid.NewGuid();
        integration.CreatedAt = DateTime.UtcNow;
        integration.UpdatedAt = DateTime.UtcNow;
        _context.Integrations.Add(integration);
        await _context.SaveChangesAsync();
        return integration;
    }

    public async Task<Integration> UpdateAsync(Integration integration)
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

