namespace Flowtap_Application.Interfaces;

/// <summary>
/// Service for authorization checks
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Checks if the current user is an Employee with Owner role
    /// </summary>
    /// <param name="userAccountId">The user account ID</param>
    /// <returns>True if user is an Employee with Owner role, false otherwise</returns>
    Task<bool> IsEmployeeOwnerAsync(Guid userAccountId);
}

