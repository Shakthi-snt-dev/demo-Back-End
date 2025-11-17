using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;
using System.Security.Claims;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Stores Controller - Manage stores
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class StoresController : ControllerBase
{
    private readonly IStoreService _storeService;
    private readonly ILogger<StoresController> _logger;

    public StoresController(
        IStoreService storeService,
        ILogger<StoresController> logger)
    {
        _storeService = storeService;
        _logger = logger;
    }

    /// <summary>
    /// Get all stores for the authenticated user
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<StoreResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<StoreResponseDto>>>> GetAll()
    {
        // Extract appUserId from JWT token
        var appUserIdClaim = User.FindFirst("app_user_id")?.Value;
        if (string.IsNullOrEmpty(appUserIdClaim) || !Guid.TryParse(appUserIdClaim, out var appUserId))
        {
            return BadRequest(ApiResponseDto<IEnumerable<StoreResponseDto>>.Failure("AppUserId is required. Please ensure you are authenticated.", null));
        }

        var result = await _storeService.GetStoresByAppUserIdAsync(appUserId);
        return Ok(ApiResponseDto<IEnumerable<StoreResponseDto>>.Success(result, "Stores retrieved successfully"));
    }

    /// <summary>
    /// Get store by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<StoreResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<StoreResponseDto>>> GetById(Guid id)
    {
        var result = await _storeService.GetStoreByIdAsync(id);
        return Ok(ApiResponseDto<StoreResponseDto>.Success(result, "Store retrieved successfully"));
    }

    /// <summary>
    /// Get stores by type
    /// </summary>
    [HttpGet("type/{storeType}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<StoreResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<StoreResponseDto>>>> GetByType(string storeType)
    {
        var result = await _storeService.GetStoresByTypeAsync(storeType);
        return Ok(ApiResponseDto<IEnumerable<StoreResponseDto>>.Success(result, "Stores retrieved successfully"));
    }

    /// <summary>
    /// Create a new store
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<StoreResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<StoreResponseDto>>> Create([FromBody] CreateStoreRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<StoreResponseDto>.Failure("Invalid request data", null));
        }

        // Extract appUserId from JWT token
        var appUserIdClaim = User.FindFirst("app_user_id")?.Value;
        if (string.IsNullOrEmpty(appUserIdClaim) || !Guid.TryParse(appUserIdClaim, out var appUserId))
        {
            return BadRequest(ApiResponseDto<StoreResponseDto>.Failure("AppUserId is required. Please ensure you are authenticated.", null));
        }

        request.AppUserId = appUserId;
        var result = await _storeService.CreateStoreAsync(request);
        return Ok(ApiResponseDto<StoreResponseDto>.Success(result, "Store created successfully"));
    }

    /// <summary>
    /// Update store
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<StoreResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<StoreResponseDto>>> Update(Guid id, [FromBody] UpdateStoreRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<StoreResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _storeService.UpdateStoreAsync(id, request);
        return Ok(ApiResponseDto<StoreResponseDto>.Success(result, "Store updated successfully"));
    }

    /// <summary>
    /// Delete store
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<object?>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object?>>> Delete(Guid id)
    {
        await _storeService.DeleteStoreAsync(id);
        return Ok(ApiResponseDto<object?>.Success(null, "Store deleted successfully"));
    }
}

