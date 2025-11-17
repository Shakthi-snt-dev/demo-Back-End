using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ISettingsService _settingsService;
    private readonly ILogger<UserController> _logger;

    public UserController(
        ISettingsService settingsService,
        ILogger<UserController> logger)
    {
        _settingsService = settingsService;
        _logger = logger;
    }

    /// <summary>
    /// Get user profile
    /// </summary>
    [HttpGet("profile")]
    [ProducesResponseType(typeof(ApiResponseDto<UserProfileDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<UserProfileDto>>> GetProfile([FromQuery] Guid appUserId)
    {
        var result = await _settingsService.GetUserProfileAsync(appUserId);
        return Ok(ApiResponseDto<UserProfileDto>.Success(result, "Profile retrieved successfully"));
    }

    /// <summary>
    /// Update user profile
    /// </summary>
    [HttpPut("profile")]
    [ProducesResponseType(typeof(ApiResponseDto<UserProfileDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<UserProfileDto>>> UpdateProfile(
        [FromQuery] Guid appUserId,
        [FromBody] UpdateUserProfileRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<UserProfileDto>.Failure("Invalid request data", null));
        }

        var result = await _settingsService.UpdateUserProfileAsync(appUserId, request);
        return Ok(ApiResponseDto<UserProfileDto>.Success(result, "Profile updated successfully"));
    }

    /// <summary>
    /// Upload user avatar
    /// </summary>
    [HttpPost("avatar")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> UploadAvatar([FromForm] IFormFile avatar, [FromQuery] Guid appUserId)
    {
        // TODO: Implement file upload logic
        await Task.CompletedTask; // Placeholder for future async implementation
        return Ok(ApiResponseDto<object>.Success(new { avatar = "avatar-url-here" }, "Avatar uploaded successfully"));
    }

    /// <summary>
    /// Change password
    /// </summary>
    [HttpPost("change-password")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> ChangePassword(
        [FromQuery] Guid appUserId,
        [FromBody] UpdateSecuritySettingsRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<object>.Failure("Invalid request data", null));
        }

        var result = await _settingsService.UpdatePasswordAsync(appUserId, request);
        if (!result)
        {
            return Ok(ApiResponseDto<object>.Failure("Failed to update password", null));
        }

        return Ok(ApiResponseDto<object?>.Success(null, "Password changed successfully"));
    }
}

