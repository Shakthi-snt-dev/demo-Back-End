using FlowTap.Api.Models;
using FlowTap.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeDto = FlowTap.Api.Services.EmployeeDto;
using CreateEmployeeRequest = FlowTap.Api.Services.CreateEmployeeRequest;
using RoleDto = FlowTap.Api.Services.RoleDto;
using CreateRoleRequest = FlowTap.Api.Services.CreateRoleRequest;
using UpdateRoleRequest = FlowTap.Api.Services.UpdateRoleRequest;

namespace FlowTap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeDto>>> GetEmployees()
    {
        var employees = await _employeeService.GetEmployeesAsync();
        return Ok(employees);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee([FromBody] CreateEmployeeRequest request)
    {
        try
        {
            var employee = await _employeeService.CreateEmployeeAsync(request);
            return Ok(new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                RoleName = employee.Role.Name ?? string.Empty,
                StoreId = employee.StoreId,
                Status = employee.Status.ToString(),
                CreatedAt = employee.CreatedAt
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("roles")]
    public async Task<ActionResult<List<RoleDto>>> GetRoles()
    {
        var roles = await _employeeService.GetRolesAsync();
        return Ok(roles);
    }

    [HttpPost("roles")]
    public async Task<ActionResult<RoleDto>> CreateRole([FromBody] CreateRoleRequest request)
    {
        try
        {
            var role = await _employeeService.CreateRoleAsync(request);
            return Ok(new RoleDto
            {
                Id = role.Id,
                Name = role.Name ?? string.Empty,
                Permissions = role.Permissions,
                IsSuperUser = role.IsSuperUser
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("roles/{id}")]
    public async Task<ActionResult<RoleDto>> UpdateRole(Guid id, [FromBody] UpdateRoleRequest request)
    {
        try
        {
            var role = await _employeeService.UpdateRoleAsync(id, request);
            return Ok(new RoleDto
            {
                Id = role.Id,
                Name = role.Name ?? string.Empty,
                Permissions = role.Permissions,
                IsSuperUser = role.IsSuperUser
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

