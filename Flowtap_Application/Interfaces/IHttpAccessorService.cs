using Flowtap_Application.Services;

namespace Flowtap_Application.Interfaces;

/// <summary>
/// Interface for HTTP Accessor Service
/// Provides methods to extract token and user information from HTTP requests
/// </summary>
public interface IHttpAccessorService
{
    /// <summary>
    /// Gets the raw token string from the Authorization header
    /// </summary>
    string? GetToken();

    /// <summary>
    /// Gets the current user's claims from the token
    /// </summary>
    System.Security.Claims.ClaimsPrincipal? GetUser();

    /// <summary>
    /// Gets a specific claim value from the current user
    /// </summary>
    string? GetClaim(string claimType);

    /// <summary>
    /// Gets the UserAccount ID (NameIdentifier claim)
    /// </summary>
    Guid? GetUserAccountId();

    /// <summary>
    /// Gets the AppUser ID from the token
    /// </summary>
    Guid? GetAppUserId();

    /// <summary>
    /// Gets the Store ID from the token
    /// </summary>
    Guid? GetStoreId();

    /// <summary>
    /// Gets the user's email from the token
    /// </summary>
    string? GetUserEmail();

    /// <summary>
    /// Gets the user's username from the token
    /// </summary>
    string? GetUsername();

    /// <summary>
    /// Gets the user type from the token
    /// </summary>
    string? GetUserType();

    /// <summary>
    /// Checks if the user's email is verified
    /// </summary>
    bool IsEmailVerified();

    /// <summary>
    /// Checks if the user is authenticated
    /// </summary>
    bool IsAuthenticated();

    /// <summary>
    /// Gets all token information along with request data
    /// </summary>
    TokenInfo GetTokenInfo();

    /// <summary>
    /// Gets request data along with token information
    /// </summary>
    RequestData GetRequestData();

    /// <summary>
    /// Logs request data with token information (for debugging)
    /// </summary>
    void LogRequestData();
}

