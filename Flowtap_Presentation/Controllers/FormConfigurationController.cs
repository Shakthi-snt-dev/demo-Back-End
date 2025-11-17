using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Form Configuration Controller - Provides dynamic form field configurations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FormConfigurationController : ControllerBase
{
    private readonly IFormConfigurationService _formConfigurationService;
    private readonly ILogger<FormConfigurationController> _logger;

    public FormConfigurationController(
        IFormConfigurationService formConfigurationService,
        ILogger<FormConfigurationController> logger)
    {
        _formConfigurationService = formConfigurationService;
        _logger = logger;
    }

    /// <summary>
    /// Get form configuration by form ID
    /// </summary>
    [HttpGet("{formId}")]
    [ProducesResponseType(typeof(ApiResponseDto<FormConfigurationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<FormConfigurationDto>>> GetFormConfiguration(
        string formId,
        [FromQuery] Guid? appUserId = null,
        [FromQuery] Guid? storeId = null)
    {
        var result = await _formConfigurationService.GetFormConfigurationAsync(formId, appUserId, storeId);
        return Ok(ApiResponseDto<FormConfigurationDto>.Success(result, "Form configuration retrieved successfully"));
    }

    /// <summary>
    /// Get user profile form configuration
    /// </summary>
    [HttpGet("user-profile/{appUserId}")]
    [ProducesResponseType(typeof(ApiResponseDto<FormConfigurationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<FormConfigurationDto>>> GetUserProfileForm(Guid appUserId)
    {
        var result = await _formConfigurationService.GetUserProfileFormConfigurationAsync(appUserId);
        return Ok(ApiResponseDto<FormConfigurationDto>.Success(result, "User profile form configuration retrieved successfully"));
    }

    /// <summary>
    /// Get store settings form configuration
    /// </summary>
    [HttpGet("store-settings/{storeId}")]
    [ProducesResponseType(typeof(ApiResponseDto<FormConfigurationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<FormConfigurationDto>>> GetStoreSettingsForm(Guid storeId)
    {
        var result = await _formConfigurationService.GetStoreSettingsFormConfigurationAsync(storeId);
        return Ok(ApiResponseDto<FormConfigurationDto>.Success(result, "Store settings form configuration retrieved successfully"));
    }

    /// <summary>
    /// Get employee form configuration
    /// </summary>
    [HttpGet("employee")]
    [ProducesResponseType(typeof(ApiResponseDto<FormConfigurationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<FormConfigurationDto>>> GetEmployeeForm([FromQuery] Guid? storeId = null)
    {
        var result = await _formConfigurationService.GetEmployeeFormConfigurationAsync(storeId);
        return Ok(ApiResponseDto<FormConfigurationDto>.Success(result, "Employee form configuration retrieved successfully"));
    }
}

