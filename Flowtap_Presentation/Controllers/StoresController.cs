using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Presentation.Attributes;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Stores Controller - Manage stores
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AppUserOrOwner]
public class StoresController : ControllerBase
{
    private readonly IStoreService _storeService;
    private readonly IHttpAccessorService _httpAccessorService;
    private readonly ILogger<StoresController> _logger;

    public StoresController(
        IStoreService storeService,
        IHttpAccessorService httpAccessorService,
        ILogger<StoresController> logger)
    {
        _storeService = storeService;
        _httpAccessorService = httpAccessorService;
        _logger = logger;
    }

    /// <summary>
    /// Get stores for the current user (ID and Name only) - for dropdown selection
    /// </summary>
    /// <returns>List of stores with ID and Name</returns>
    [HttpGet("list")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<StoreListItemDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<StoreListItemDto>>>> GetStoresList()
    {
        try
        {
            // Get UserAccount ID from token
            var userAccountId = _httpAccessorService.GetUserAccountId();
            if (!userAccountId.HasValue)
            {
                return Unauthorized(ApiResponseDto<IEnumerable<StoreListItemDto>>.Failure("User not authenticated or UserAccount ID not found", null));
            }

            var stores = await _storeService.GetStoresForCurrentUserAsync(userAccountId.Value);
            return Ok(ApiResponseDto<IEnumerable<StoreListItemDto>>.Success(stores, $"Retrieved {stores.Count()} stores"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<IEnumerable<StoreListItemDto>>.Failure(ex.Message, null));
        }
        catch (System.InvalidOperationException ex)
        {
            return BadRequest(ApiResponseDto<IEnumerable<StoreListItemDto>>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving stores list");
            return BadRequest(ApiResponseDto<IEnumerable<StoreListItemDto>>.Failure("Error retrieving stores", null));
        }
    }
}

