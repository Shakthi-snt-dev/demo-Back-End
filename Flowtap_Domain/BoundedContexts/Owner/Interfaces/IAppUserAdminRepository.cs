using Flowtap_Domain.BoundedContexts.Owner.Entities;

namespace Flowtap_Domain.BoundedContexts.Owner.Interfaces;

public interface IAppUserAdminRepository
{
    Task<AppUserAdmin?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AppUserAdmin?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppUserAdmin>> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default);
    Task<AppUserAdmin?> GetPrimaryOwnerAsync(Guid appUserId, CancellationToken cancellationToken = default);
    Task<AppUserAdmin> AddAsync(AppUserAdmin admin, CancellationToken cancellationToken = default);
    Task UpdateAsync(AppUserAdmin admin, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}

