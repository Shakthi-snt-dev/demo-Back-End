using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;
using System.Security.Claims;

namespace Flowtap_Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly ISettingsService _settingsService;
    private readonly ILogger<SettingsController> _logger;

    public SettingsController(
        ISettingsService settingsService,
        ILogger<SettingsController> logger)
    {
        _settingsService = settingsService;
        _logger = logger;
    }

    /// <summary>
    /// Get UserAccount ID from JWT token claims
    /// </summary>
    private Guid? GetUserAccountIdFromToken()
    {
        // Check if user is authenticated
        if (User?.Identity == null || !User.Identity.IsAuthenticated)
        {
            _logger.LogWarning("User is not authenticated. Identity: {Identity}, IsAuthenticated: {IsAuth}", 
                User?.Identity?.Name ?? "null", User?.Identity?.IsAuthenticated ?? false);
            return null;
        }

        // Since we configured NameClaimType = "nameid" in JWT options, 
        // the nameid claim should be mapped to User.Identity.Name
        // But we'll also try direct claim lookup as fallback
        var userIdClaim = User.Identity.Name  // This should contain nameid value due to NameClaimType mapping
            ?? User.FindFirst("nameid")?.Value  // Try JWT short name directly
            ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value  // Try standard claim type
            ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value; // Try full URI

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            _logger.LogWarning("Unable to extract UserAccount ID from token. Identity.Name: {Name}, Available claims: {Claims}", 
                User.Identity.Name ?? "null",
                string.Join(", ", User.Claims.Select(c => $"{c.Type}={c.Value}")));
            return null;
        }
        
        _logger.LogDebug("Extracted UserAccount ID from token: {UserId}", userId);
        return userId;
    }

    /// <summary>
    /// Get all settings
    /// </summary>
    [HttpGet("user/{appUserId}")]
    [ProducesResponseType(typeof(ApiResponseDto<SettingsResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<SettingsResponseDto>>> GetSettings(Guid appUserId)
    {
        var result = await _settingsService.GetSettingsAsync(appUserId);
        return Ok(ApiResponseDto<SettingsResponseDto>.Success(result, "Settings retrieved successfully"));
    }

    /// <summary>
    /// Update general settings
    /// </summary>
    [HttpPut("user/{appUserId}/general")]
    [ProducesResponseType(typeof(ApiResponseDto<GeneralSettingsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<GeneralSettingsDto>>> UpdateGeneralSettings(Guid appUserId, [FromBody] UpdateGeneralSettingsRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<GeneralSettingsDto>.Failure("Invalid request data", null));
        }

        var result = await _settingsService.UpdateGeneralSettingsAsync(appUserId, request);
        return Ok(ApiResponseDto<GeneralSettingsDto>.Success(result, "General settings updated successfully"));
    }

    /// <summary>
    /// Update inventory settings
    /// </summary>
    [HttpPut("store/{storeId}/inventory")]
    [ProducesResponseType(typeof(ApiResponseDto<InventorySettingsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<InventorySettingsDto>>> UpdateInventorySettings(Guid storeId, [FromBody] UpdateInventorySettingsRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<InventorySettingsDto>.Failure("Invalid request data", null));
        }

        var result = await _settingsService.UpdateInventorySettingsAsync(storeId, request);
        return Ok(ApiResponseDto<InventorySettingsDto>.Success(result, "Inventory settings updated successfully"));
    }

    /// <summary>
    /// Update notification settings
    /// </summary>
    [HttpPut("user/{appUserId}/notifications")]
    [ProducesResponseType(typeof(ApiResponseDto<NotificationSettingsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<NotificationSettingsDto>>> UpdateNotificationSettings(Guid appUserId, [FromBody] UpdateNotificationSettingsRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<NotificationSettingsDto>.Failure("Invalid request data", null));
        }

        var result = await _settingsService.UpdateNotificationSettingsAsync(appUserId, request);
        return Ok(ApiResponseDto<NotificationSettingsDto>.Success(result, "Notification settings updated successfully"));
    }

    /// <summary>
    /// Update payment settings
    /// </summary>
    [HttpPut("store/{storeId}/payment")]
    [ProducesResponseType(typeof(ApiResponseDto<PaymentSettingsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<PaymentSettingsDto>>> UpdatePaymentSettings(Guid storeId, [FromBody] UpdatePaymentSettingsRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<PaymentSettingsDto>.Failure("Invalid request data", null));
        }

        var result = await _settingsService.UpdatePaymentSettingsAsync(storeId, request);
        return Ok(ApiResponseDto<PaymentSettingsDto>.Success(result, "Payment settings updated successfully"));
    }

    /// <summary>
    /// Update password
    /// </summary>
    [HttpPut("user/{appUserId}/password")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> UpdatePassword(Guid appUserId, [FromBody] UpdateSecuritySettingsRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<object>.Failure("Invalid request data", null));
        }

        var result = await _settingsService.UpdatePasswordAsync(appUserId, request);
        return Ok(ApiResponseDto<object?>.Success(null, "Password updated successfully"));
    }

    /// <summary>
    /// Enable two-factor authentication
    /// </summary>
    [HttpPost("user/{appUserId}/two-factor")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> EnableTwoFactor(Guid appUserId)
    {
        var result = await _settingsService.EnableTwoFactorAsync(appUserId);
        return Ok(ApiResponseDto<object?>.Success(null, "Two-factor authentication enabled successfully"));
    }

    /// <summary>
    /// Get store settings
    /// </summary>
    [HttpGet("store/{storeId}")]
    [ProducesResponseType(typeof(ApiResponseDto<StoreSettingsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<StoreSettingsDto>>> GetStoreSettings(Guid storeId)
    {
        var result = await _settingsService.GetStoreSettingsAsync(storeId);
        return Ok(ApiResponseDto<StoreSettingsDto>.Success(result, "Store settings retrieved successfully"));
    }

    /// <summary>
    /// Update store settings
    /// </summary>
    [HttpPut("store/{storeId}")]
    [ProducesResponseType(typeof(ApiResponseDto<StoreSettingsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<StoreSettingsDto>>> UpdateStoreSettings(
        Guid storeId,
        [FromBody] UpdateStoreSettingsRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<StoreSettingsDto>.Failure("Invalid request data", null));
        }

        var result = await _settingsService.UpdateStoreSettingsAsync(storeId, request);
        return Ok(ApiResponseDto<StoreSettingsDto>.Success(result, "Store settings updated successfully"));
    }

    /// <summary>
    /// Reset API key for store
    /// </summary>
    [HttpPost("store/{storeId}/api-key/reset")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> ResetApiKey(Guid storeId)
    {
        var result = await _settingsService.ResetApiKeyAsync(storeId);
        return Ok(ApiResponseDto<object>.Success(new { apiKey = result }, "API key reset successfully"));
    }

    /// <summary>
    /// Check user type by email (AppUser, Employee, or AdminUser)
    /// </summary>
    [HttpPost("check-user-type")]
    [ProducesResponseType(typeof(ApiResponseDto<CheckUserTypeResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<CheckUserTypeResponseDto>>> CheckUserType([FromBody] CheckUserTypeRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<CheckUserTypeResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _settingsService.CheckUserTypeByEmailAsync(request.Email);
        return Ok(ApiResponseDto<CheckUserTypeResponseDto>.Success(result, "User type checked successfully"));
    }

    /// <summary>
    /// Get AppUser profile settings
    /// </summary>
    [HttpGet("profile")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ProducesResponseType(typeof(ApiResponseDto<AppUserProfileResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<AppUserProfileResponseDto>>> GetAppUserProfile()
    {
        var userAccountId = GetUserAccountIdFromToken();
        if (!userAccountId.HasValue)
        {
            return Unauthorized(ApiResponseDto<AppUserProfileResponseDto>.Failure("User not authenticated", null));
        }

        var result = await _settingsService.GetAppUserProfileByUserAccountIdAsync(userAccountId.Value);
        return Ok(ApiResponseDto<AppUserProfileResponseDto>.Success(result, "Profile retrieved successfully"));
    }

    /// <summary>
    /// Create or update AppUser profile settings (POST)
    /// </summary>
    [HttpPost("profile")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ProducesResponseType(typeof(ApiResponseDto<AppUserProfileResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<AppUserProfileResponseDto>>> CreateOrUpdateAppUserProfile(
        [FromBody] AppUserProfileRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<AppUserProfileResponseDto>.Failure("Invalid request data", null));
        }

        var userAccountId = GetUserAccountIdFromToken();
        if (!userAccountId.HasValue)
        {
            return Unauthorized(ApiResponseDto<AppUserProfileResponseDto>.Failure("User not authenticated", null));
        }

        var result = await _settingsService.CreateOrUpdateAppUserProfileByUserAccountIdAsync(userAccountId.Value, request);
        return Ok(ApiResponseDto<AppUserProfileResponseDto>.Success(result, "Profile saved successfully"));
    }

    /// <summary>
    /// Update AppUser profile settings (PUT)
    /// </summary>
    [HttpPut("profile")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ProducesResponseType(typeof(ApiResponseDto<AppUserProfileResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<AppUserProfileResponseDto>>> UpdateAppUserProfile(
        [FromBody] AppUserProfileRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<AppUserProfileResponseDto>.Failure("Invalid request data", null));
        }

        var userAccountId = GetUserAccountIdFromToken();
        if (!userAccountId.HasValue)
        {
            return Unauthorized(ApiResponseDto<AppUserProfileResponseDto>.Failure("User not authenticated", null));
        }

        var result = await _settingsService.CreateOrUpdateAppUserProfileByUserAccountIdAsync(userAccountId.Value, request);
        return Ok(ApiResponseDto<AppUserProfileResponseDto>.Success(result, "Profile updated successfully"));
    }
}

