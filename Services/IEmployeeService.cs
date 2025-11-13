using FlowTap.Api.Models;

namespace FlowTap.Api.Services;

public interface IEmployeeService
{
    Task<List<EmployeeDto>> GetEmployeesAsync();
    Task<Employee> CreateEmployeeAsync(CreateEmployeeRequest request);
    Task<List<RoleDto>> GetRolesAsync();
    Task<ApplicationRole> CreateRoleAsync(CreateRoleRequest request);
    Task<ApplicationRole> UpdateRoleAsync(Guid id, UpdateRoleRequest request);
}

public class CreateEmployeeRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public Guid? StoreId { get; set; }
}

public class CreateRoleRequest
{
    public string Name { get; set; } = string.Empty;
    public Dictionary<string, Dictionary<string, bool>> Permissions { get; set; } = new();
    public bool IsSuperUser { get; set; } = false;
}

public class UpdateRoleRequest
{
    public string? Name { get; set; }
    public Dictionary<string, Dictionary<string, bool>>? Permissions { get; set; }
    public bool? IsSuperUser { get; set; }
}

public class EmployeeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public Guid? StoreId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Dictionary<string, Dictionary<string, bool>> Permissions { get; set; } = new();
    public bool IsSuperUser { get; set; }
}

