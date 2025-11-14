using Flowtap_Domain.BoundedContexts.Identity.Entities;

namespace Flowtap_Domain.BoundedContexts.Identity.Interfaces;

public interface IUserAccountRepository
{
    Task<UserAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserAccount?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<UserAccount?> GetByVerificationTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<UserAccount?> GetByExternalProviderAsync(string provider, string providerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserAccount>> GetByUserTypeAsync(SharedKernel.Enums.UserType userType, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserAccount>> GetActiveAccountsAsync(CancellationToken cancellationToken = default);
    Task<UserAccount> AddAsync(UserAccount userAccount, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserAccount userAccount, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default);
}

