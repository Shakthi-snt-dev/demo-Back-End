using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OnboardingController : ControllerBase
{
    private readonly IRegistrationService _registrationService;
    private readonly ILogger<OnboardingController> _logger;

    public OnboardingController(
        IRegistrationService registrationService,
        ILogger<OnboardingController> logger)
    {
        _registrationService = registrationService;
        _logger = logger;
    }

    /// <summary>
    /// Complete onboarding step 1: Profile settings
    /// </summary>
    [HttpPost("step-1/{appUserId}")]
    [ProducesResponseType(typeof(ApiResponseDto<OnboardingResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponseDto<OnboardingResponseDto>>> CompleteStep1(
        [FromRoute] Guid appUserId,
        [FromBody] OnboardingStep1RequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<OnboardingResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _registrationService.CompleteOnboardingStep1Async(appUserId, request);
        return Ok(ApiResponseDto<OnboardingResponseDto>.Success(result, result.Message));
    }

    /// <summary>
    /// Complete onboarding step 2: Store settings
    /// </summary>
    [HttpPost("step-2/{appUserId}")]
    [ProducesResponseType(typeof(ApiResponseDto<OnboardingResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponseDto<OnboardingResponseDto>>> CompleteStep2(
        [FromRoute] Guid appUserId,
        [FromBody] OnboardingStep2RequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<OnboardingResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _registrationService.CompleteOnboardingStep2Async(appUserId, request);
        return Ok(ApiResponseDto<OnboardingResponseDto>.Success(result, result.Message));
    }

    /// <summary>
    /// Complete onboarding step 3: Choose action (Dive in or Demo)
    /// </summary>
    [HttpPost("step-3/{appUserId}")]
    [ProducesResponseType(typeof(ApiResponseDto<OnboardingResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponseDto<OnboardingResponseDto>>> CompleteStep3(
        [FromRoute] Guid appUserId,
        [FromBody] OnboardingStep3RequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<OnboardingResponseDto>.Failure("Invalid request data", null));
        }

        if (request.Action.ToLower() != "dive_in" && request.Action.ToLower() != "demo")
        {
            return Ok(ApiResponseDto<OnboardingResponseDto>.Failure("Action must be 'dive_in' or 'demo'", null));
        }

        var result = await _registrationService.CompleteOnboardingStep3Async(appUserId, request);
        return Ok(ApiResponseDto<OnboardingResponseDto>.Success(result, result.Message));
    }

    /// <summary>
    /// Upgrade from trial to paid subscription
    /// </summary>
    [HttpPost("upgrade-subscription/{appUserId}")]
    [ProducesResponseType(typeof(ApiResponseDto<UpgradeToSubscriptionResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponseDto<UpgradeToSubscriptionResponseDto>>> UpgradeToSubscription(
        [FromRoute] Guid appUserId,
        [FromBody] UpgradeToSubscriptionRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<UpgradeToSubscriptionResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _registrationService.UpgradeToSubscriptionAsync(appUserId, request);
        return Ok(ApiResponseDto<UpgradeToSubscriptionResponseDto>.Success(result, result.Message));
    }
}

