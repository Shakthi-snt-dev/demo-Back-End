using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

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
}

