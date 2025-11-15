using Microsoft.EntityFrameworkCore;
using Flowtap_Domain.BoundedContexts.Identity.Entities;
using Flowtap_Domain.BoundedContexts.Identity.Interfaces;
using Flowtap_Domain.SharedKernel.Enums;
using Flowtap_Infrastructure.Data;

namespace Flowtap_Infrastructure.Repositories;

public class UserAccountRepository : IUserAccountRepository
{
    private readonly AppDbContext _context;

    public UserAccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.UserAccounts
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<UserAccount?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.UserAccounts
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<UserAccount?> GetByVerificationTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _context.UserAccounts
            .FirstOrDefaultAsync(u => u.EmailVerificationToken == token, cancellationToken);
    }

    public async Task<UserAccount?> GetByExternalProviderAsync(string provider, string providerId, CancellationToken cancellationToken = default)
    {
        return await _context.UserAccounts
            .FirstOrDefaultAsync(u => u.ExternalProvider == provider && u.ExternalProviderId == providerId, cancellationToken);
    }

    public async Task<IEnumerable<UserAccount>> GetByUserTypeAsync(UserType userType, CancellationToken cancellationToken = default)
    {
        return await _context.UserAccounts
            .Where(u => u.UserType == userType)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserAccount>> GetActiveAccountsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.UserAccounts
            .Where(u => u.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<UserAccount> AddAsync(UserAccount userAccount, CancellationToken cancellationToken = default)
    {
        await _context.UserAccounts.AddAsync(userAccount, cancellationToken);
        var changes = await _context.SaveChangesAsync(cancellationToken);
        Console.WriteLine($"[REPOSITORY] UserAccountRepository.AddAsync - SaveChanges returned: {changes} changes");
        Console.WriteLine($"[REPOSITORY] Database type: {_context.Database.ProviderName}");
        return userAccount;
    }

    public async Task UpdateAsync(UserAccount userAccount, CancellationToken cancellationToken = default)
    {
        _context.UserAccounts.Update(userAccount);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userAccount = await GetByIdAsync(id, cancellationToken);
        if (userAccount != null)
        {
            _context.UserAccounts.Remove(userAccount);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.UserAccounts
            .AnyAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.UserAccounts
            .AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username))
            return false;

        return await _context.UserAccounts
            .AnyAsync(u => u.Username == username, cancellationToken);
    }
}

