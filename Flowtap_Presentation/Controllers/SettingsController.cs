using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly ISettingsService _settingsService;
    private readonly IHttpAccessorService _httpAccessorService;
    private readonly ILogger<SettingsController> _logger;

    public SettingsController(
        ISettingsService settingsService,
        IHttpAccessorService httpAccessorService,
        ILogger<SettingsController> logger)
    {
        _settingsService = settingsService;
        _httpAccessorService = httpAccessorService;
        _logger = logger;
    }

   
    
    [HttpGet("profile")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ProducesResponseType(typeof(ApiResponseDto<AppUserProfileResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<AppUserProfileResponseDto>>> GetAppUserProfile()
    {
        var userAccountId = _httpAccessorService.GetUserAccountId();
        if (!userAccountId.HasValue)
        {
            return Unauthorized(ApiResponseDto<AppUserProfileResponseDto>.Failure("User not authenticated", null));
        }

        var result = await _settingsService.GetAppUserProfileByUserAccountIdAsync(userAccountId.Value);
        return Ok(ApiResponseDto<AppUserProfileResponseDto>.Success(result, "Profile retrieved successfully"));
    }

   
  
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

        var userAccountId = _httpAccessorService.GetUserAccountId();
        if (!userAccountId.HasValue)
        {
            return Unauthorized(ApiResponseDto<AppUserProfileResponseDto>.Failure("User not authenticated", null));
        }

        var result = await _settingsService.CreateOrUpdateAppUserProfileByUserAccountIdAsync(userAccountId.Value, request);
        return Ok(ApiResponseDto<AppUserProfileResponseDto>.Success(result, "Profile updated successfully"));
    }
}

