using Microsoft.AspNetCore.Mvc;
using Flowtap_Application.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Presentation.Attributes;

namespace Flowtap_Presentation.Controllers;

/// <summary>
/// Employees Controller - Manage employees by store
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AppUserOrOwner]
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
    /// Get employees by store ID with optional role filter
    /// </summary>
    /// <param name="storeId">Store ID</param>
    /// <param name="role">Optional role filter (default: "all" to get all roles)</param>
    /// <returns>List of employees for the store</returns>
    [HttpGet("store/{storeId}")]
    [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<EmployeeResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<EmployeeResponseDto>>>> GetEmployeesByStore(
        Guid storeId,
        [FromQuery] string? role = "all")
    {
        try
        {
            var employees = await _employeeService.GetEmployeesByStoreIdAsync(storeId, role);
            return Ok(ApiResponseDto<IEnumerable<EmployeeResponseDto>>.Success(employees, $"Retrieved {employees.Count()} employees for store"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<IEnumerable<EmployeeResponseDto>>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employees for store {StoreId}", storeId);
            return BadRequest(ApiResponseDto<IEnumerable<EmployeeResponseDto>>.Failure("Error retrieving employees", null));
        }
    }

    /// <summary>
    /// Create a new employee for a store
    /// </summary>
    /// <param name="request">Employee creation request (must include StoreId)</param>
    /// <returns>Created employee</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<EmployeeResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponseDto<EmployeeResponseDto>>> CreateEmployee(
        [FromBody] CreateEmployeeRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<EmployeeResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var employee = await _employeeService.CreateEmployeeAsync(request);
            return Ok(ApiResponseDto<EmployeeResponseDto>.Success(employee, "Employee created successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<EmployeeResponseDto>.Failure(ex.Message, null));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponseDto<EmployeeResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee for store {StoreId}", request.StoreId);
            return BadRequest(ApiResponseDto<EmployeeResponseDto>.Failure("Error creating employee", null));
        }
    }

    /// <summary>
    /// Get employee by ID
    /// </summary>
    /// <param name="id">Employee ID</param>
    /// <returns>Employee details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<EmployeeResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<EmployeeResponseDto>>> GetEmployeeById(Guid id)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            return Ok(ApiResponseDto<EmployeeResponseDto>.Success(employee, "Employee retrieved successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<EmployeeResponseDto>.Failure(ex.Message, null));
        }
    }

    /// <summary>
    /// Update employee
    /// </summary>
    /// <param name="id">Employee ID</param>
    /// <param name="request">Employee update request</param>
    /// <returns>Updated employee</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<EmployeeResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<EmployeeResponseDto>>> UpdateEmployee(
        Guid id,
        [FromBody] UpdateEmployeeRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseDto<EmployeeResponseDto>.Failure("Invalid request data", null));
        }

        try
        {
            var employee = await _employeeService.UpdateEmployeeAsync(id, request);
            return Ok(ApiResponseDto<EmployeeResponseDto>.Success(employee, "Employee updated successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<EmployeeResponseDto>.Failure(ex.Message, null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee {EmployeeId}", id);
            return BadRequest(ApiResponseDto<EmployeeResponseDto>.Failure("Error updating employee", null));
        }
    }

    /// <summary>
    /// Delete employee
    /// </summary>
    /// <param name="id">Employee ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteEmployee(Guid id)
    {
        try
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);
            return Ok(ApiResponseDto<bool>.Success(result, "Employee deleted successfully"));
        }
        catch (Flowtap_Domain.Exceptions.EntityNotFoundException ex)
        {
            return NotFound(ApiResponseDto<bool>.Failure(ex.Message, false));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employee {EmployeeId}", id);
            return BadRequest(ApiResponseDto<bool>.Failure("Error deleting employee", false));
        }
    }
}

