using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Roles Controller - Manage roles and permissions
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly ILogger<RolesController> _logger;

    public RolesController(
        IRoleService roleService,
        ILogger<RolesController> logger)
    {
        _roleService = roleService;
        _logger = logger;
    }

    /// <summary>
    /// Get all roles
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<RoleResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<RoleResponseDto>>>> GetAll()
    {
        var result = await _roleService.GetAllRolesAsync();
        return Ok(ApiResponseDto<IEnumerable<RoleResponseDto>>.Success(result, "Roles retrieved successfully"));
    }

    /// <summary>
    /// Get role by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<RoleResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<RoleResponseDto>>> GetById(Guid id)
    {
        var result = await _roleService.GetRoleByIdAsync(id);
        return Ok(ApiResponseDto<RoleResponseDto>.Success(result, "Role retrieved successfully"));
    }

    /// <summary>
    /// Get role by name
    /// </summary>
    [HttpGet("name/{name}")]
    [ProducesResponseType(typeof(ApiResponseDto<RoleResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<RoleResponseDto>>> GetByName(string name)
    {
        var result = await _roleService.GetRoleByNameAsync(name);
        if (result == null)
            return NotFound(ApiResponseDto<RoleResponseDto>.Failure("Role not found", null));

        return Ok(ApiResponseDto<RoleResponseDto>.Success(result, "Role retrieved successfully"));
    }

    /// <summary>
    /// Create a new role
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<RoleResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<RoleResponseDto>>> Create([FromBody] CreateRoleRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<RoleResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _roleService.CreateRoleAsync(request);
        return Ok(ApiResponseDto<RoleResponseDto>.Success(result, "Role created successfully"));
    }

    /// <summary>
    /// Update role
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<RoleResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<RoleResponseDto>>> Update(Guid id, [FromBody] UpdateRoleRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<RoleResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _roleService.UpdateRoleAsync(id, request);
        return Ok(ApiResponseDto<RoleResponseDto>.Success(result, "Role updated successfully"));
    }

    /// <summary>
    /// Delete role
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<object?>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object?>>> Delete(Guid id)
    {
        await _roleService.DeleteRoleAsync(id);
        return Ok(ApiResponseDto<object?>.Success(null, "Role deleted successfully"));
    }
}

