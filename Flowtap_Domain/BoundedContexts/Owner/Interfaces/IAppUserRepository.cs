using Flowtap_Domain.BoundedContexts.Owner.Entities;

namespace Flowtap_Domain.BoundedContexts.Owner.Interfaces;

public interface IAppUserRepository
{
    Task<AppUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AppUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<AppUser?> GetBySubscriptionIdAsync(Guid subscriptionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppUser>> GetByTrialStatusAsync(SharedKernel.Enums.TrialStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppUser>> GetActiveTrialsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AppUser>> GetExpiringTrialsAsync(DateTime beforeDate, CancellationToken cancellationToken = default);
    Task<AppUser> AddAsync(AppUser appUser, CancellationToken cancellationToken = default);
    Task UpdateAsync(AppUser appUser, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
}

