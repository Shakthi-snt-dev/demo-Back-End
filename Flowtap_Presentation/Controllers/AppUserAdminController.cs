using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// AppUserAdmin Controller - Manage business owners (AppUserAdmin)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AppUserAdminController : ControllerBase
{
    private readonly IAppUserAdminService _appUserAdminService;
    private readonly ILogger<AppUserAdminController> _logger;

    public AppUserAdminController(
        IAppUserAdminService appUserAdminService,
        ILogger<AppUserAdminController> logger)
    {
        _appUserAdminService = appUserAdminService;
        _logger = logger;
    }

    /// <summary>
    /// Get all business owners (AppUserAdmin) for an AppUser
    /// </summary>
    [HttpGet("appuser/{appUserId}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<AppUserAdminResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<AppUserAdminResponseDto>>>> GetByAppUserId(Guid appUserId)
    {
        var result = await _appUserAdminService.GetAppUserAdminsByAppUserIdAsync(appUserId);
        return Ok(ApiResponseDto<IEnumerable<AppUserAdminResponseDto>>.Success(result, "Business owners retrieved successfully"));
    }

    /// <summary>
    /// Get business owner by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<AppUserAdminResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<AppUserAdminResponseDto>>> GetById(Guid id)
    {
        var result = await _appUserAdminService.GetAppUserAdminByIdAsync(id);
        return Ok(ApiResponseDto<AppUserAdminResponseDto>.Success(result, "Business owner retrieved successfully"));
    }

    /// <summary>
    /// Create a new business owner (AppUserAdmin)
    /// This will also create an Employee record with Owner role if CreateAsEmployee is true
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<AppUserAdminResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<AppUserAdminResponseDto>>> Create(
        [FromQuery] Guid appUserId,
        [FromBody] CreateAppUserAdminRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<AppUserAdminResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _appUserAdminService.CreateAppUserAdminAsync(appUserId, request);
        return Ok(ApiResponseDto<AppUserAdminResponseDto>.Success(result, "Business owner created successfully"));
    }

    /// <summary>
    /// Update business owner
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<AppUserAdminResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<AppUserAdminResponseDto>>> Update(
        Guid id,
        [FromBody] CreateAppUserAdminRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<AppUserAdminResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _appUserAdminService.UpdateAppUserAdminAsync(id, request);
        return Ok(ApiResponseDto<AppUserAdminResponseDto>.Success(result, "Business owner updated successfully"));
    }

    /// <summary>
    /// Delete business owner
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> Delete(Guid id)
    {
        var deleted = await _appUserAdminService.DeleteAppUserAdminAsync(id);
        if (!deleted)
        {
            return Ok(ApiResponseDto<object>.Failure("Business owner not found", null));
        }

        return Ok(ApiResponseDto<object?>.Success(null, "Business owner deleted successfully"));
    }
}

