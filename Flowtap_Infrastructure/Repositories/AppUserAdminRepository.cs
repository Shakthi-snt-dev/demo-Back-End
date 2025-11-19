using Flowtap_Domain.BoundedContexts.Owner.Entities;
using Flowtap_Domain.BoundedContexts.Owner.Interfaces;

namespace Flowtap_Infrastructure.Repositories;

/// <summary>
/// Stub implementation - AppUserAdmin is no longer part of DbContext
/// All methods return empty/null results
/// </summary>
public class AppUserAdminRepository : IAppUserAdminRepository
{
    public AppUserAdminRepository()
    {
    }

    public Task<AppUserAdmin?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<AppUserAdmin?>(null);
    }

    public Task<AppUserAdmin?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<AppUserAdmin?>(null);
    }

    public Task<IEnumerable<AppUserAdmin>> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<AppUserAdmin>>(new List<AppUserAdmin>());
    }

    public Task<AppUserAdmin?> GetPrimaryOwnerAsync(Guid appUserId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<AppUserAdmin?>(null);
    }

    public Task<AppUserAdmin> AddAsync(AppUserAdmin admin, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(admin);
    }

    public Task UpdateAsync(AppUserAdmin admin, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }
}

