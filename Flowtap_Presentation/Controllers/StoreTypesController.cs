using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Store Types Controller - Manage store types
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class StoreTypesController : ControllerBase
{
    private readonly IStoreService _storeService;
    private readonly ILogger<StoreTypesController> _logger;

    public StoreTypesController(
        IStoreService storeService,
        ILogger<StoreTypesController> logger)
    {
        _storeService = storeService;
        _logger = logger;
    }

    /// <summary>
    /// Get all store types
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<StoreTypeResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<StoreTypeResponseDto>>>> GetAll()
    {
        var result = await _storeService.GetStoreTypesAsync();
        return Ok(ApiResponseDto<IEnumerable<StoreTypeResponseDto>>.Success(result, "Store types retrieved successfully"));
    }

    /// <summary>
    /// Create a new store type
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<StoreTypeResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<StoreTypeResponseDto>>> Create([FromBody] CreateStoreTypeRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<StoreTypeResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _storeService.CreateStoreTypeAsync(request);
        return Ok(ApiResponseDto<StoreTypeResponseDto>.Success(result, "Store type created successfully"));
    }

    /// <summary>
    /// Delete store type
    /// </summary>
    [HttpDelete("{name}")]
    [ProducesResponseType(typeof(ApiResponseDto<object?>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object?>>> Delete(string name)
    {
        await _storeService.DeleteStoreTypeAsync(name);
        return Ok(ApiResponseDto<object?>.Success(null, "Store type deleted successfully"));
    }
}

