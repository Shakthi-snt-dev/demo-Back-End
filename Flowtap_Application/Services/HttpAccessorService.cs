using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using Flowtap_Application.Interfaces;

namespace Flowtap_Application.Services;

/// <summary>
/// HTTP Accessor Service
/// Extracts token and user information from HTTP requests
/// Provides methods to get user data along with token information
/// </summary>
public class HttpAccessorService : IHttpAccessorService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HttpAccessorService> _logger;

    public HttpAccessorService(
        IHttpContextAccessor httpContextAccessor,
        ILogger<HttpAccessorService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    /// <summary>
    /// Gets the current HTTP context
    /// </summary>
    public HttpContext? GetHttpContext()
    {
        return _httpContextAccessor.HttpContext;
    }

    /// <summary>
    /// Gets the raw token string from the Authorization header
    /// </summary>
    public string? GetToken()
    {
        var httpContext = GetHttpContext();
        if (httpContext == null)
            return null;

        var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(authHeader))
            return null;

        // Extract token from "Bearer {token}" format
        if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return authHeader.Substring("Bearer ".Length).Trim();
        }

        return authHeader;
    }

    /// <summary>
    /// Gets the JWT token as a JwtSecurityToken object
    /// </summary>
    public JwtSecurityToken? GetJwtToken()
    {
        var tokenString = GetToken();
        if (string.IsNullOrWhiteSpace(tokenString))
            return null;

        try
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(tokenString))
            {
                return handler.ReadJwtToken(tokenString);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to parse JWT token");
        }

        return null;
    }

    /// <summary>
    /// Gets the current user's claims from the token
    /// </summary>
    public ClaimsPrincipal? GetUser()
    {
        var httpContext = GetHttpContext();
        return httpContext?.User;
    }

    /// <summary>
    /// Gets a specific claim value from the current user
    /// </summary>
    public string? GetClaim(string claimType)
    {
        var user = GetUser();
        return user?.FindFirst(claimType)?.Value;
    }

    /// <summary>
    /// Gets the UserAccount ID (NameIdentifier claim)
    /// Tries multiple claim types to find the user ID
    /// </summary>
    public Guid? GetUserAccountId()
    {
        var user = GetUser();
        if (user?.Identity == null || !user.Identity.IsAuthenticated)
        {
            _logger.LogWarning("User is not authenticated. Identity: {Identity}, IsAuthenticated: {IsAuth}", 
                user?.Identity?.Name ?? "null", user?.Identity?.IsAuthenticated ?? false);
            return null;
        }

        // Since we configured NameClaimType = "nameid" in JWT options, 
        // the nameid claim should be mapped to User.Identity.Name
        // But we'll also try direct claim lookup as fallback
        var userIdClaim = user.Identity.Name  // This should contain nameid value due to NameClaimType mapping
            ?? user.FindFirst("nameid")?.Value  // Try JWT short name directly
            ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value  // Try standard claim type
            ?? user.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value; // Try full URI

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            _logger.LogWarning("Unable to extract UserAccount ID from token. Identity.Name: {Name}, Available claims: {Claims}", 
                user.Identity.Name ?? "null",
                string.Join(", ", user.Claims.Select(c => $"{c.Type}={c.Value}")));
            return null;
        }
        
        _logger.LogDebug("Extracted UserAccount ID from token: {UserId}", userId);
        return userId;
    }

    /// <summary>
    /// Gets the AppUser ID from the token
    /// </summary>
    public Guid? GetAppUserId()
    {
        var appUserIdString = GetClaim("app_user_id");
        if (Guid.TryParse(appUserIdString, out var appUserId))
        {
            return appUserId;
        }
        return null;
    }

    /// <summary>
    /// Gets the Store ID from the token
    /// </summary>
    public Guid? GetStoreId()
    {
        var storeIdString = GetClaim("store_id");
        if (Guid.TryParse(storeIdString, out var storeId))
        {
            return storeId;
        }
        return null;
    }

    /// <summary>
    /// Gets the user's email from the token
    /// </summary>
    public string? GetUserEmail()
    {
        return GetClaim(ClaimTypes.Email);
    }

    /// <summary>
    /// Gets the user's username from the token
    /// </summary>
    public string? GetUsername()
    {
        return GetClaim(ClaimTypes.Name);
    }

    /// <summary>
    /// Gets the user type from the token
    /// </summary>
    public string? GetUserType()
    {
        return GetClaim("user_type");
    }

    /// <summary>
    /// Checks if the user's email is verified
    /// </summary>
    public bool IsEmailVerified()
    {
        var isVerifiedString = GetClaim("is_email_verified");
        return bool.TryParse(isVerifiedString, out var isVerified) && isVerified;
    }

    /// <summary>
    /// Checks if the user is authenticated
    /// </summary>
    public bool IsAuthenticated()
    {
        var user = GetUser();
        return user?.Identity?.IsAuthenticated ?? false;
    }

    /// <summary>
    /// Gets all token information along with request data
    /// </summary>
    public TokenInfo GetTokenInfo()
    {
        var token = GetToken();
        var jwtToken = GetJwtToken();
        var user = GetUser();

        return new TokenInfo
        {
            HasToken = !string.IsNullOrWhiteSpace(token),
            TokenPreview = token != null && token.Length > 20 
                ? $"{token.Substring(0, 20)}..." 
                : token,
            TokenLength = token?.Length ?? 0,
            UserAccountId = GetUserAccountId(),
            AppUserId = GetAppUserId(),
            StoreId = GetStoreId(),
            Email = GetUserEmail(),
            Username = GetUsername(),
            UserType = GetUserType(),
            IsEmailVerified = IsEmailVerified(),
            IsAuthenticated = IsAuthenticated(),
            TokenExpiry = jwtToken?.ValidTo,
            TokenIssuedAt = jwtToken?.ValidFrom,
            AllClaims = user?.Claims?.ToDictionary(c => c.Type, c => c.Value) ?? new Dictionary<string, string>()
        };
    }

    /// <summary>
    /// Gets request data along with token information
    /// </summary>
    public RequestData GetRequestData()
    {
        var httpContext = GetHttpContext();
        if (httpContext == null)
        {
            return new RequestData();
        }

        var request = httpContext.Request;
        var tokenInfo = GetTokenInfo();

        return new RequestData
        {
            Method = request.Method,
            Path = request.Path.Value ?? string.Empty,
            QueryString = request.QueryString.Value ?? string.Empty,
            Headers = request.Headers.ToDictionary(
                h => h.Key,
                h => string.Join(", ", h.Value)
            ),
            TokenInfo = tokenInfo,
            Timestamp = DateTime.UtcNow,
            RemoteIpAddress = httpContext.Connection.RemoteIpAddress?.ToString(),
            UserAgent = request.Headers["User-Agent"].ToString()
        };
    }

    /// <summary>
    /// Logs request data with token information (for debugging)
    /// </summary>
    public void LogRequestData()
    {
        var requestData = GetRequestData();
        var tokenInfo = GetTokenInfo();

        _logger.LogInformation(
            "Request: {Method} {Path} | User: {Email} ({UserId}) | AppUser: {AppUserId} | Store: {StoreId} | Token: {TokenPreview}",
            requestData.Method,
            requestData.Path,
            tokenInfo.Email ?? "Anonymous",
            tokenInfo.UserAccountId,
            tokenInfo.AppUserId,
            tokenInfo.StoreId,
            tokenInfo.TokenPreview ?? "No token"
        );
    }
}

/// <summary>
/// Token information model
/// </summary>
public class TokenInfo
{
    public bool HasToken { get; set; }
    public string? TokenPreview { get; set; }
    public int TokenLength { get; set; }
    public Guid? UserAccountId { get; set; }
    public Guid? AppUserId { get; set; }
    public Guid? StoreId { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? UserType { get; set; }
    public bool IsEmailVerified { get; set; }
    public bool IsAuthenticated { get; set; }
    public DateTime? TokenExpiry { get; set; }
    public DateTime? TokenIssuedAt { get; set; }
    public Dictionary<string, string> AllClaims { get; set; } = new();
}

/// <summary>
/// Request data model with token information
/// </summary>
public class RequestData
{
    public string Method { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string QueryString { get; set; } = string.Empty;
    public Dictionary<string, string> Headers { get; set; } = new();
    public TokenInfo TokenInfo { get; set; } = new();
    public DateTime Timestamp { get; set; }
    public string? RemoteIpAddress { get; set; }
    public string UserAgent { get; set; } = string.Empty;
}

