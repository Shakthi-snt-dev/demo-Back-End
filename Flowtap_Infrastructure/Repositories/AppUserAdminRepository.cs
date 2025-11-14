using Microsoft.EntityFrameworkCore;
using Flowtap_Domain.BoundedContexts.Owner.Entities;
using Flowtap_Domain.BoundedContexts.Owner.Interfaces;
using Flowtap_Infrastructure.Data;

namespace Flowtap_Infrastructure.Repositories;

public class AppUserAdminRepository : IAppUserAdminRepository
{
    private readonly AppDbContext _context;

    public AppUserAdminRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AppUserAdmin?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AppUserAdmins
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<AppUserAdmin?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.AppUserAdmins
            .FirstOrDefaultAsync(a => a.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<AppUserAdmin>> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default)
    {
        return await _context.AppUserAdmins
            .Where(a => a.AppUserId == appUserId)
            .ToListAsync(cancellationToken);
    }

    public async Task<AppUserAdmin?> GetPrimaryOwnerAsync(Guid appUserId, CancellationToken cancellationToken = default)
    {
        return await _context.AppUserAdmins
            .FirstOrDefaultAsync(a => a.AppUserId == appUserId && a.IsPrimaryOwner, cancellationToken);
    }

    public async Task<AppUserAdmin> AddAsync(AppUserAdmin admin, CancellationToken cancellationToken = default)
    {
        await _context.AppUserAdmins.AddAsync(admin, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return admin;
    }

    public async Task UpdateAsync(AppUserAdmin admin, CancellationToken cancellationToken = default)
    {
        _context.AppUserAdmins.Update(admin);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var admin = await GetByIdAsync(id, cancellationToken);
        if (admin != null)
        {
            _context.AppUserAdmins.Remove(admin);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AppUserAdmins
            .AnyAsync(a => a.Id == id, cancellationToken);
    }
}

