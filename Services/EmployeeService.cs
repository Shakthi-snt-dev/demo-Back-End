using FlowTap.Api.Data;
using FlowTap.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlowTap.Api.Services;

public class EmployeeService : IEmployeeService
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public EmployeeService(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager)
    {
        _context = context;
        _roleManager = roleManager;
    }

    public async Task<List<EmployeeDto>> GetEmployeesAsync()
    {
        var employees = await _context.Employees
            .Include(e => e.Role)
            .OrderBy(e => e.Name)
            .ToListAsync();

        return employees.Select(e => new EmployeeDto
        {
            Id = e.Id,
            Name = e.Name,
            Email = e.Email,
            RoleName = e.Role?.Name ?? string.Empty,
            StoreId = e.StoreId,
            Status = e.Status.ToString(),
            CreatedAt = e.CreatedAt
        }).ToList();
    }

    public async Task<Employee> CreateEmployeeAsync(CreateEmployeeRequest request)
    {
        var employee = new Employee
        {
            Name = request.Name,
            Email = request.Email,
            RoleId = request.RoleId,
            StoreId = request.StoreId,
            Status = EmployeeStatus.Active,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return employee;
    }

    public async Task<List<RoleDto>> GetRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();

        return roles.Select(r => new RoleDto
        {
            Id = r.Id,
            Name = r.Name ?? string.Empty,
            Permissions = r.Permissions,
            IsSuperUser = r.IsSuperUser
        }).ToList();
    }

    public async Task<ApplicationRole> CreateRoleAsync(CreateRoleRequest request)
    {
        var role = new ApplicationRole
        {
            Name = request.Name,
            Permissions = request.Permissions,
            IsSuperUser = request.IsSuperUser
        };

        var result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        return role;
    }

    public async Task<ApplicationRole> UpdateRoleAsync(Guid id, UpdateRoleRequest request)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role == null)
            throw new Exception("Role not found");

        if (!string.IsNullOrEmpty(request.Name))
            role.Name = request.Name;
        if (request.Permissions != null)
            role.Permissions = request.Permissions;
        if (request.IsSuperUser.HasValue)
            role.IsSuperUser = request.IsSuperUser.Value;

        var result = await _roleManager.UpdateAsync(role);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        return role;
    }
}

