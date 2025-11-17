using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Employees Controller - Alias for Employee operations (plural route)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(
        IEmployeeService employeeService,
        ILogger<EmployeesController> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

    /// <summary>
    /// Get all employees
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<EmployeeResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<EmployeeResponseDto>>>> GetAll(
        [FromQuery] int? page = null,
        [FromQuery] int? limit = null)
    {
        var result = await _employeeService.GetAllEmployeesAsync();

        // Apply pagination if provided
        if (page.HasValue && limit.HasValue)
        {
            result = result.Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();
        }
        
        return Ok(ApiResponseDto<IEnumerable<EmployeeResponseDto>>.Success(result, "Employees retrieved successfully"));
    }

    /// <summary>
    /// Get employee by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<EmployeeResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<EmployeeResponseDto>>> GetById(Guid id)
    {
        var result = await _employeeService.GetEmployeeByIdAsync(id);
        return Ok(ApiResponseDto<EmployeeResponseDto>.Success(result, "Employee retrieved successfully"));
    }

    /// <summary>
    /// Create a new employee
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<EmployeeResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<EmployeeResponseDto>>> Create([FromBody] CreateEmployeeRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<EmployeeResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _employeeService.CreateEmployeeAsync(request);
        return Ok(ApiResponseDto<EmployeeResponseDto>.Success(result, "Employee created successfully"));
    }

    /// <summary>
    /// Update employee
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<EmployeeResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<EmployeeResponseDto>>> Update(Guid id, [FromBody] UpdateEmployeeRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<EmployeeResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _employeeService.UpdateEmployeeAsync(id, request);
        return Ok(ApiResponseDto<EmployeeResponseDto>.Success(result, "Employee updated successfully"));
    }

    /// <summary>
    /// Delete employee
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> Delete(Guid id)
    {
        var deleted = await _employeeService.DeleteEmployeeAsync(id);
        if (!deleted)
        {
            return Ok(ApiResponseDto<object>.Failure("Employee not found", null));
        }

        return Ok(ApiResponseDto<object?>.Success(null, "Employee deleted successfully"));
    }

    /// <summary>
    /// Update employee role
    /// </summary>
    [HttpPatch("{id}/role")]
    [ProducesResponseType(typeof(ApiResponseDto<EmployeeResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<EmployeeResponseDto>>> UpdateRole(
        Guid id,
        [FromBody] UpdateEmployeeRoleRequestDto request)
    {
        // Get employee first to update role
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            return Ok(ApiResponseDto<EmployeeResponseDto>.Failure("Employee not found", null));
        }

        // Update employee with new role
        var updateRequest = new UpdateEmployeeRequestDto
        {
            Role = request.Role
        };

        var result = await _employeeService.UpdateEmployeeAsync(id, updateRequest);
        return Ok(ApiResponseDto<EmployeeResponseDto>.Success(result, "Employee role updated successfully"));
    }

    /// <summary>
    /// Add a partner/admin to a store (only owner can add partners)
    /// </summary>
    [HttpPost("partners")]
    [ProducesResponseType(typeof(ApiResponseDto<EmployeeResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<EmployeeResponseDto>>> AddPartner(
        [FromQuery] Guid ownerAppUserId,
        [FromBody] AddPartnerRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<EmployeeResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _employeeService.AddPartnerAsync(ownerAppUserId, request);
        return Ok(ApiResponseDto<EmployeeResponseDto>.Success(result, "Partner/Admin added successfully"));
    }
}

// DTO for employee role update
public class UpdateEmployeeRoleRequestDto
{
    public string Role { get; set; } = string.Empty;
}

