using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Domain.DtoModel;

namespace Flowtap_Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(
        IEmployeeService employeeService,
        ILogger<EmployeeController> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new employee
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<EmployeeResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<EmployeeResponseDto>>> CreateEmployee([FromBody] CreateEmployeeRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<EmployeeResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _employeeService.CreateEmployeeAsync(request);
        return Ok(ApiResponseDto<EmployeeResponseDto>.Success(result, "Employee created successfully"));
    }

    /// <summary>
    /// Get employee by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<EmployeeResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<EmployeeResponseDto>>> GetEmployee(Guid id)
    {
        var result = await _employeeService.GetEmployeeByIdAsync(id);
        return Ok(ApiResponseDto<EmployeeResponseDto>.Success(result, "Employee retrieved successfully"));
    }

    /// <summary>
    /// Get all employees
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<EmployeeResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<EmployeeResponseDto>>>> GetAllEmployees()
    {
        var result = await _employeeService.GetAllEmployeesAsync();
        return Ok(ApiResponseDto<IEnumerable<EmployeeResponseDto>>.Success(result, "Employees retrieved successfully"));
    }

    /// <summary>
    /// Get employees by store ID
    /// </summary>
    [HttpGet("store/{storeId}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<EmployeeResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<EmployeeResponseDto>>>> GetEmployeesByStore(Guid storeId)
    {
        var result = await _employeeService.GetEmployeesByStoreIdAsync(storeId);
        return Ok(ApiResponseDto<IEnumerable<EmployeeResponseDto>>.Success(result, "Employees retrieved successfully"));
    }

    /// <summary>
    /// Get employees by role
    /// </summary>
    [HttpGet("role/{role}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<EmployeeResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<EmployeeResponseDto>>>> GetEmployeesByRole(string role)
    {
        var result = await _employeeService.GetEmployeesByRoleAsync(role);
        return Ok(ApiResponseDto<IEnumerable<EmployeeResponseDto>>.Success(result, "Employees retrieved successfully"));
    }

    /// <summary>
    /// Get active employees
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<EmployeeResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<EmployeeResponseDto>>>> GetActiveEmployees()
    {
        var result = await _employeeService.GetActiveEmployeesAsync();
        return Ok(ApiResponseDto<IEnumerable<EmployeeResponseDto>>.Success(result, "Active employees retrieved successfully"));
    }

    /// <summary>
    /// Update employee
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<EmployeeResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<EmployeeResponseDto>>> UpdateEmployee(Guid id, [FromBody] UpdateEmployeeRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(ApiResponseDto<EmployeeResponseDto>.Failure("Invalid request data", null));
        }

        var result = await _employeeService.UpdateEmployeeAsync(id, request);
        return Ok(ApiResponseDto<EmployeeResponseDto>.Success(result, "Employee updated successfully"));
    }

    /// <summary>
    /// Activate employee
    /// </summary>
    [HttpPost("{id}/activate")]
    [ProducesResponseType(typeof(ApiResponseDto<EmployeeResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<EmployeeResponseDto>>> ActivateEmployee(Guid id)
    {
        var result = await _employeeService.ActivateEmployeeAsync(id);
        return Ok(ApiResponseDto<EmployeeResponseDto>.Success(result, "Employee activated successfully"));
    }

    /// <summary>
    /// Deactivate employee
    /// </summary>
    [HttpPost("{id}/deactivate")]
    [ProducesResponseType(typeof(ApiResponseDto<EmployeeResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<EmployeeResponseDto>>> DeactivateEmployee(Guid id)
    {
        var result = await _employeeService.DeactivateEmployeeAsync(id);
        return Ok(ApiResponseDto<EmployeeResponseDto>.Success(result, "Employee deactivated successfully"));
    }

    /// <summary>
    /// Delete employee
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponseDto<object>>> DeleteEmployee(Guid id)
    {
        var deleted = await _employeeService.DeleteEmployeeAsync(id);
        if (!deleted)
        {
            return Ok(ApiResponseDto<object>.Failure("Employee not found", null));
        }

        return Ok(ApiResponseDto<object>.Success(null, "Employee deleted successfully"));
    }
}

