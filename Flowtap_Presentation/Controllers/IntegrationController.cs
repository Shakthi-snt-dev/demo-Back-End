using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IntegrationController : ControllerBase
{
    private readonly IIntegrationService _integrationService;
    private readonly ILogger<IntegrationController> _logger;

    public IntegrationController(
        IIntegrationService integrationService,
        ILogger<IntegrationController> logger)
    {
        _integrationService = integrationService;
        _logger = logger;
    }

    /// <summary>
    /// Create QuickBooks integration
    /// </summary>
    [HttpPost("quickbooks")]
    [ProducesResponseType(typeof(ApiResponseDto<IntegrationResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IntegrationResponseDto>>> CreateQuickBooksIntegration([FromBody] CreateQuickBooksIntegrationRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<IntegrationResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _integrationService.CreateQuickBooksIntegrationAsync(request);
        return Ok(ApiResponseDto<IntegrationResponseDto>.Success(result, "QuickBooks integration created successfully"));
    }

    /// <summary>
    /// Create Shopify integration
    /// </summary>
    [HttpPost("shopify")]
    [ProducesResponseType(typeof(ApiResponseDto<IntegrationResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IntegrationResponseDto>>> CreateShopifyIntegration([FromBody] CreateShopifyIntegrationRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<IntegrationResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _integrationService.CreateShopifyIntegrationAsync(request);
        return Ok(ApiResponseDto<IntegrationResponseDto>.Success(result, "Shopify integration created successfully"));
    }

    /// <summary>
    /// Get integration by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<IntegrationResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IntegrationResponseDto>>> GetIntegration(Guid id)
    {
        var result = await _integrationService.GetIntegrationByIdAsync(id);
        return Ok(ApiResponseDto<IntegrationResponseDto>.Success(result, "Integration retrieved successfully"));
    }

    /// <summary>
    /// Get all integrations for user
    /// </summary>
    [HttpGet("user/{appUserId}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<IntegrationResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<IntegrationResponseDto>>>> GetIntegrationsByUser(Guid appUserId)
    {
        var result = await _integrationService.GetIntegrationsByAppUserIdAsync(appUserId);
        return Ok(ApiResponseDto<IEnumerable<IntegrationResponseDto>>.Success(result, "Integrations retrieved successfully"));
    }

    /// <summary>
    /// Get integration by type
    /// </summary>
    [HttpGet("user/{appUserId}/type/{type}")]
    [ProducesResponseType(typeof(ApiResponseDto<IntegrationResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IntegrationResponseDto>>> GetIntegrationByType(Guid appUserId, string type)
    {
        var result = await _integrationService.GetIntegrationByTypeAsync(appUserId, type);
        if (result == null)
        {
            return Ok(ApiResponseDto<IntegrationResponseDto>.Failure("Integration not found", null));
        }

        return Ok(ApiResponseDto<IntegrationResponseDto>.Success(result, "Integration retrieved successfully"));
    }

    /// <summary>
    /// Update integration
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<IntegrationResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IntegrationResponseDto>>> UpdateIntegration(Guid id, [FromBody] UpdateIntegrationRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<IntegrationResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _integrationService.UpdateIntegrationAsync(id, request);
        return Ok(ApiResponseDto<IntegrationResponseDto>.Success(result, "Integration updated successfully"));
    }

    /// <summary>
    /// Connect integration
    /// </summary>
    [HttpPost("{id}/connect")]
    [ProducesResponseType(typeof(ApiResponseDto<IntegrationResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IntegrationResponseDto>>> ConnectIntegration(Guid id)
    {
        var result = await _integrationService.ConnectIntegrationAsync(id);
        return Ok(ApiResponseDto<IntegrationResponseDto>.Success(result, "Integration connected successfully"));
    }

    /// <summary>
    /// Disconnect integration
    /// </summary>
    [HttpPost("{id}/disconnect")]
    [ProducesResponseType(typeof(ApiResponseDto<IntegrationResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IntegrationResponseDto>>> DisconnectIntegration(Guid id)
    {
        var result = await _integrationService.DisconnectIntegrationAsync(id);
        return Ok(ApiResponseDto<IntegrationResponseDto>.Success(result, "Integration disconnected successfully"));
    }

    /// <summary>
    /// Enable integration
    /// </summary>
    [HttpPost("{id}/enable")]
    [ProducesResponseType(typeof(ApiResponseDto<IntegrationResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IntegrationResponseDto>>> EnableIntegration(Guid id)
    {
        var result = await _integrationService.EnableIntegrationAsync(id);
        return Ok(ApiResponseDto<IntegrationResponseDto>.Success(result, "Integration enabled successfully"));
    }

    /// <summary>
    /// Disable integration
    /// </summary>
    [HttpPost("{id}/disable")]
    [ProducesResponseType(typeof(ApiResponseDto<IntegrationResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IntegrationResponseDto>>> DisableIntegration(Guid id)
    {
        var result = await _integrationService.DisableIntegrationAsync(id);
        return Ok(ApiResponseDto<IntegrationResponseDto>.Success(result, "Integration disabled successfully"));
    }

    /// <summary>
    /// Sync integration
    /// </summary>
    [HttpPost("{id}/sync")]
    [ProducesResponseType(typeof(ApiResponseDto<SyncResultDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<SyncResultDto>>> SyncIntegration(Guid id, [FromBody] SyncIntegrationRequestDto? request = null)
    {
        var result = await _integrationService.SyncIntegrationAsync(id, request);
        return Ok(ApiResponseDto<SyncResultDto>.Success(result, "Integration synced successfully"));
    }

    /// <summary>
    /// Delete integration
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> DeleteIntegration(Guid id)
    {
        var deleted = await _integrationService.DeleteIntegrationAsync(id);
        if (!deleted)
        {
            return Ok(ApiResponseDto<object>.Failure("Integration not found", null));
        }

        return Ok(ApiResponseDto<object?>.Success(null, "Integration deleted successfully"));
    }
}

