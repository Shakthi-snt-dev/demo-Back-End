using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Presentation.Attributes;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Profile Controller - Manage user profile settings
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AppUserOrOwner]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly IHttpAccessorService _httpAccessorService;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(
        IProfileService profileService,
        IHttpAccessorService httpAccessorService,
        ILogger<ProfileController> logger)
    {
        _profileService = profileService;
        _httpAccessorService = httpAccessorService;
        _logger = logger;
    }

    /// <summary>
    /// Get user profile
    /// </summary>
    /// <returns>User profile</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<AppUserProfileResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponseDto<AppUserProfileResponseDto>>> GetProfile()
    {
        var userAccountId = _httpAccessorService.GetUserAccountId();
        if (!userAccountId.HasValue)
        {
            return Unauthorized(ApiResponseDto<AppUserProfileResponseDto>.Failure("User not authenticated", null));
        }

        try
        {
            var result = await _profileService.GetAppUserProfileByUserAccountIdAsync(userAccountId.Value);
            return Ok(ApiResponseDto<AppUserProfileResponseDto>.Success(result, "Profile retrieved successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<AppUserProfileResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Update user profile
    /// </summary>
    /// <param name="request">Profile update request</param>
    /// <returns>Updated profile</returns>
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponseDto<AppUserProfileResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponseDto<AppUserProfileResponseDto>>> UpdateProfile(
        [FromBody] AppUserProfileRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<AppUserProfileResponseDto>.Failure("Invalid request data", null));
        }

        var userAccountId = _httpAccessorService.GetUserAccountId();
        if (!userAccountId.HasValue)
        {
            return Unauthorized(ApiResponseDto<AppUserProfileResponseDto>.Failure("User not authenticated", null));
        }

        try
        {
            var result = await _profileService.CreateOrUpdateAppUserProfileByUserAccountIdAsync(userAccountId.Value, request);
            return Ok(ApiResponseDto<AppUserProfileResponseDto>.Success(result, "Profile updated successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<AppUserProfileResponseDto>.Failure(ex.Message, null));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponseDto<AppUserProfileResponseDto>.Failure(ex.Message, null));
        }
    }
}

