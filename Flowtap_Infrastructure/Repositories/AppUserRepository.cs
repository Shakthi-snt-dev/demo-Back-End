using Microsoft.EntityFrameworkCore;
using Flowtap_Domain.BoundedContexts.Owner.Entities;
using Flowtap_Domain.BoundedContexts.Owner.Interfaces;
using Flowtap_Domain.SharedKernel.Enums;
using Flowtap_Infrastructure.Data;

namespace Flowtap_Infrastructure.Repositories;

public class AppUserRepository : IAppUserRepository
{
    private readonly AppDbContext _context;

    public AppUserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AppUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AppUsers
            .Include(u => u.Admins)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<AppUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.AppUsers
            .Include(u => u.Admins)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<AppUser?> GetBySubscriptionIdAsync(Guid subscriptionId, CancellationToken cancellationToken = default)
    {
        return await _context.AppUsers
            .Include(u => u.Admins)
            .FirstOrDefaultAsync(u => u.SubscriptionId == subscriptionId, cancellationToken);
    }

    public async Task<IEnumerable<AppUser>> GetByTrialStatusAsync(TrialStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.AppUsers
            .Where(u => u.TrialStatus == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AppUser>> GetActiveTrialsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AppUsers
            .Where(u => u.TrialStatus == TrialStatus.Active && 
                       u.TrialEndDate.HasValue && 
                       u.TrialEndDate.Value >= DateTime.UtcNow)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AppUser>> GetExpiringTrialsAsync(DateTime beforeDate, CancellationToken cancellationToken = default)
    {
        return await _context.AppUsers
            .Where(u => u.TrialStatus == TrialStatus.Active && 
                       u.TrialEndDate.HasValue && 
                       u.TrialEndDate.Value <= beforeDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<AppUser> AddAsync(AppUser appUser, CancellationToken cancellationToken = default)
    {
        await _context.AppUsers.AddAsync(appUser, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return appUser;
    }

    public async Task UpdateAsync(AppUser appUser, CancellationToken cancellationToken = default)
    {
        _context.AppUsers.Update(appUser);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var appUser = await GetByIdAsync(id, cancellationToken);
        if (appUser != null)
        {
            _context.AppUsers.Remove(appUser);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AppUsers
            .AnyAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.AppUsers
            .AnyAsync(u => u.Email == email, cancellationToken);
    }
}

