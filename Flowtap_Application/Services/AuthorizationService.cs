using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.HR.Interfaces;
using Flowtap_Domain.BoundedContexts.Identity.Interfaces;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

/// <summary>
/// Service for authorization checks
/// </summary>
public class AuthorizationService : IAuthorizationService
{
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<AuthorizationService> _logger;

    public AuthorizationService(
        IUserAccountRepository userAccountRepository,
        IEmployeeRepository employeeRepository,
        ILogger<AuthorizationService> logger)
    {
        _userAccountRepository = userAccountRepository;
        _employeeRepository = employeeRepository;
        _logger = logger;
    }

    /// <summary>
    /// Checks if the current user is an Employee with Owner role
    /// </summary>
    public async Task<bool> IsEmployeeOwnerAsync(Guid userAccountId)
    {
        try
        {
            // Get UserAccount to check EmployeeId
            var userAccount = await _userAccountRepository.GetByIdAsync(userAccountId);
            if (userAccount == null)
            {
                _logger.LogWarning("User account not found: {UserAccountId}", userAccountId);
                return false;
            }

            // Check if user has an EmployeeId and the employee has Owner role
            if (userAccount.EmployeeId.HasValue)
            {
                var employee = await _employeeRepository.GetByIdAsync(userAccount.EmployeeId.Value);
                if (employee != null && employee.Role == "Owner" && employee.IsActive)
                {
                    return true; // Authorized - Employee with Owner role
                }
            }

            // Also check if AppUser is linked as Employee with Owner role
            if (userAccount.AppUserId.HasValue)
            {
                var employees = await _employeeRepository.GetByLinkedAppUserIdAsync(userAccount.AppUserId.Value);
                var ownerEmployee = employees.FirstOrDefault(e => e.Role == "Owner" && e.IsActive);
                if (ownerEmployee != null)
                {
                    return true; // Authorized - AppUser linked as Employee with Owner role
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking employee owner authorization for user: {UserAccountId}", userAccountId);
            return false;
        }
    }
}

