using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Store Settings Controller - Manage store settings
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class StoreSettingsController : ControllerBase
{
    private readonly IStoreSettingsService _storeSettingsService;
    private readonly IHttpAccessorService _httpAccessorService;
    private readonly ILogger<StoreSettingsController> _logger;

    public StoreSettingsController(
        IStoreSettingsService storeSettingsService,
        IHttpAccessorService httpAccessorService,
        ILogger<StoreSettingsController> logger)
    {
        _storeSettingsService = storeSettingsService;
        _httpAccessorService = httpAccessorService;
        _logger = logger;
    }

    /// <summary>
    /// Get store settings by store ID
    /// </summary>
    /// <param name="storeId">Store ID</param>
    /// <returns>Store settings</returns>
    [HttpGet("stores/{storeId}")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ProducesResponseType(typeof(ApiResponseDto<StoreSettingsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<StoreSettingsDto>>> GetStoreSettings(Guid storeId)
    {
        var userAccountId = _httpAccessorService.GetUserAccountId();
        if (!userAccountId.HasValue)
        {
            return Unauthorized(ApiResponseDto<StoreSettingsDto>.Failure("User not authenticated", null));
        }

        try
        {
            var result = await _storeSettingsService.GetStoreSettingsByUserAccountIdAsync(userAccountId.Value, storeId);
            return Ok(ApiResponseDto<StoreSettingsDto>.Success(result, "Store settings retrieved successfully"));
        }
        catch (Flowtap_Domain.Exceptions.UnauthorizedException ex)
        {
            return Unauthorized(ApiResponseDto<StoreSettingsDto>.Failure(ex.Message, null));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<StoreSettingsDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Update store settings by store ID
    /// </summary>
    /// <param name="storeId">Store ID</param>
    /// <param name="request">Store settings update request</param>
    /// <returns>Updated store settings</returns>
    [HttpPut("stores/{storeId}")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ProducesResponseType(typeof(ApiResponseDto<StoreSettingsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<StoreSettingsDto>>> UpdateStoreSettings(
        Guid storeId,
        [FromBody] UpdateStoreSettingsRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<StoreSettingsDto>.Failure("Invalid request data", null));
        }

        var userAccountId = _httpAccessorService.GetUserAccountId();
        if (!userAccountId.HasValue)
        {
            return Unauthorized(ApiResponseDto<StoreSettingsDto>.Failure("User not authenticated", null));
        }

        try
        {
            var result = await _storeSettingsService.UpdateStoreSettingsByUserAccountIdAsync(userAccountId.Value, storeId, request);
            return Ok(ApiResponseDto<StoreSettingsDto>.Success(result, "Store settings updated successfully"));
        }
        catch (Flowtap_Domain.Exceptions.UnauthorizedException ex)
        {
            return Unauthorized(ApiResponseDto<StoreSettingsDto>.Failure(ex.Message, null));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<StoreSettingsDto>.Failure(ex.Message, null));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponseDto<StoreSettingsDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Reset store API key by store ID
    /// </summary>
    /// <param name="storeId">Store ID</param>
    /// <returns>New API key</returns>
    [HttpPost("stores/{storeId}/reset-api-key")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ProducesResponseType(typeof(ApiResponseDto<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<string>>> ResetApiKey(Guid storeId)
    {
        var userAccountId = _httpAccessorService.GetUserAccountId();
        if (!userAccountId.HasValue)
        {
            return Unauthorized(ApiResponseDto<string>.Failure("User not authenticated", null));
        }

        try
        {
            var result = await _storeSettingsService.ResetApiKeyByUserAccountIdAsync(userAccountId.Value, storeId);
            return Ok(ApiResponseDto<string>.Success(result, "API key reset successfully"));
        }
        catch (Flowtap_Domain.Exceptions.UnauthorizedException ex)
        {
            return Unauthorized(ApiResponseDto<string>.Failure(ex.Message, null));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<string>.Failure(ex.Message, null));
        }
    }


    /// <summary>
    /// Send company email verification
    /// </summary>
    /// <param name="storeId">Store ID</param>
    /// <returns>Success status</returns>
    [HttpPost("stores/{storeId}/send-verification-email")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<bool>>> SendCompanyEmailVerification(Guid storeId)
    {
        var userAccountId = _httpAccessorService.GetUserAccountId();
        if (!userAccountId.HasValue)
        {
            return Unauthorized(ApiResponseDto<bool>.Failure("User not authenticated", default(bool)));
        }

        try
        {
            var result = await _storeSettingsService.SendCompanyEmailVerificationAsync(userAccountId.Value, storeId);
            return Ok(ApiResponseDto<bool>.Success(result, result ? "Verification email sent successfully" : "Failed to send verification email"));
        }
        catch (Flowtap_Domain.Exceptions.UnauthorizedException ex)
        {
            return Unauthorized(ApiResponseDto<bool>.Failure(ex.Message, default(bool)));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<bool>.Failure(ex.Message, default(bool)));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponseDto<bool>.Failure(ex.Message, default(bool)));
        }
    }

    /// <summary>
    /// Verify company email
    /// </summary>
    /// <param name="storeId">Store ID</param>
    /// <param name="verificationToken">Verification token</param>
    /// <returns>Success status</returns>
    [HttpPost("stores/{storeId}/verify-email")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<bool>>> VerifyCompanyEmail(Guid storeId, [FromQuery] string verificationToken)
    {
        var userAccountId = _httpAccessorService.GetUserAccountId();
        if (!userAccountId.HasValue)
        {
            return Unauthorized(ApiResponseDto<bool>.Failure("User not authenticated", default(bool)));
        }

        if (string.IsNullOrWhiteSpace(verificationToken))
        {
            return BadRequest(ApiResponseDto<bool>.Failure("Verification token is required", default(bool)));
        }

        try
        {
            var result = await _storeSettingsService.VerifyCompanyEmailAsync(userAccountId.Value, storeId, verificationToken);
            return Ok(ApiResponseDto<bool>.Success(result, result ? "Email verified successfully" : "Invalid verification token"));
        }
        catch (Flowtap_Domain.Exceptions.UnauthorizedException ex)
        {
            return Unauthorized(ApiResponseDto<bool>.Failure(ex.Message, default(bool)));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<bool>.Failure(ex.Message, default(bool)));
        }
    }
}

