using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IRegistrationService _registrationService;
    private readonly ILoginService _loginService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IRegistrationService registrationService,
        ILoginService loginService,
        ILogger<AuthController> logger)
    {
        _registrationService = registrationService;
        _loginService = loginService;
        _logger = logger;
    }

    /// <summary>
    /// Register a new user account
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponseDto<RegisterResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponseDto<RegisterResponseDto>>> Register([FromBody] RegisterRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<RegisterResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _registrationService.RegisterAsync(request);
        return Ok(ApiResponseDto<RegisterResponseDto>.Success(result, result.Message));
    }

    /// <summary>
    /// Verify email address using verification token
    /// </summary>
    [HttpGet("verify-email")]
    [ProducesResponseType(typeof(ApiResponseDto<VerifyEmailResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponseDto<VerifyEmailResponseDto>>> VerifyEmail([FromQuery] string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return Ok(ApiResponseDto<VerifyEmailResponseDto>.Failure("Verification token is required", null));
        }

        var result = await _registrationService.VerifyEmailAsync(token);
        
        if (!result.IsVerified)
        {
            return Ok(ApiResponseDto<VerifyEmailResponseDto>.Failure(result.Message, result));
        }

        return Ok(ApiResponseDto<VerifyEmailResponseDto>.Success(result, result.Message));
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponseDto<LoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponseDto<LoginResponseDto>>> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<LoginResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _loginService.LoginAsync(request);
        
        if (!result.Success)
        {
            return Ok(ApiResponseDto<LoginResponseDto>.Failure(result.Message, result));
        }

        return Ok(ApiResponseDto<LoginResponseDto>.Success(result, result.Message));
    }
}

