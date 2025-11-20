using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.HR.Entities;
using Flowtap_Domain.BoundedContexts.HR.Interfaces;
using Flowtap_Application.DtoModel;
using Flowtap_Application.DtoModel.Request;
using Flowtap_Application.DtoModel.Response;
using Flowtap_Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Flowtap_Application.Services;

public class RoleService : IRoleService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<RoleService> _logger;

    // In-memory storage for roles (in a real app, you'd have a Role entity and repository)
    private static readonly Dictionary<Guid, RoleResponseDto> _roles = new();
    private static readonly Dictionary<string, Guid> _roleNameToId = new();

    public RoleService(
        IEmployeeRepository employeeRepository,
        ILogger<RoleService> logger)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
        
        // Initialize with default roles if empty
        if (_roles.Count == 0)
        {
            InitializeDefaultRoles();
        }
    }

    public async Task<RoleResponseDto> CreateRoleAsync(CreateRoleRequestDto request)
    {
        if (_roleNameToId.ContainsKey(request.Name))
            throw new System.InvalidOperationException($"Role '{request.Name}' already exists");

        var role = new RoleResponseDto
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            IsSuperUser = request.IsSuperUser,
            Permissions = request.Permissions ?? new Dictionary<string, RolePermissionDto>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _roles[role.Id] = role;
        _roleNameToId[role.Name] = role.Id;

        await Task.CompletedTask;
        return role;
    }

    public async Task<RoleResponseDto> GetRoleByIdAsync(Guid id)
    {
        if (!_roles.TryGetValue(id, out var role))
            throw new EntityNotFoundException("Role", id);

        await Task.CompletedTask;
        return role;
    }

    public async Task<RoleResponseDto?> GetRoleByNameAsync(string name)
    {
        if (!_roleNameToId.TryGetValue(name, out var id))
        {
            await Task.CompletedTask;
            return null;
        }

        return await GetRoleByIdAsync(id);
    }

    public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync()
    {
        // Update employee counts by querying employees by role
        foreach (var role in _roles.Values)
        {
            try
            {
                var employees = await _employeeRepository.GetByRoleAsync(role.Name);
                role.EmployeeCount = employees.Count();
            }
            catch
            {
                // If GetByRoleAsync fails, set count to 0
                role.EmployeeCount = 0;
            }
        }

        return _roles.Values;
    }

    public async Task<RoleResponseDto> UpdateRoleAsync(Guid id, UpdateRoleRequestDto request)
    {
        if (!_roles.TryGetValue(id, out var role))
            throw new EntityNotFoundException("Role", id);

        if (request.Description != null)
            role.Description = request.Description;

        if (request.IsSuperUser.HasValue)
            role.IsSuperUser = request.IsSuperUser.Value;

        if (request.Permissions != null)
            role.Permissions = request.Permissions;

        role.UpdatedAt = DateTime.UtcNow;

        await Task.CompletedTask;
        return role;
    }

    public async Task<bool> DeleteRoleAsync(Guid id)
    {
        if (!_roles.TryGetValue(id, out var role))
            throw new EntityNotFoundException("Role", id);

        // Check if any employees use this role
        // Note: This is a simplified check - in production, you'd query the database properly
        // For now, we'll allow deletion but log a warning
        _logger.LogWarning("Role deletion check skipped - implement proper employee role check");

        _roles.Remove(id);
        _roleNameToId.Remove(role.Name);

        await Task.CompletedTask;
        return true;
    }

    private void InitializeDefaultRoles()
    {
        var defaultRoles = new[]
        {
            new RoleResponseDto
            {
                Id = Guid.NewGuid(),
                Name = "Owner",
                Description = "Store owner with full access",
                IsSuperUser = true,
                Permissions = new Dictionary<string, RolePermissionDto>
                {
                    { "pos", new RolePermissionDto { Access = true, Edit = true, Delete = true } },
                    { "inventory", new RolePermissionDto { Access = true, Edit = true, Delete = true } },
                    { "customers", new RolePermissionDto { Access = true, Edit = true, Delete = true } },
                    { "tickets", new RolePermissionDto { Access = true, Edit = true, Delete = true } },
                    { "reports", new RolePermissionDto { Access = true, Edit = true, Delete = true } },
                    { "settings", new RolePermissionDto { Access = true, Edit = true, Delete = true } }
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new RoleResponseDto
            {
                Id = Guid.NewGuid(),
                Name = "Manager",
                Description = "Store manager with management access",
                IsSuperUser = false,
                Permissions = new Dictionary<string, RolePermissionDto>
                {
                    { "pos", new RolePermissionDto { Access = true, Edit = true, Delete = false } },
                    { "inventory", new RolePermissionDto { Access = true, Edit = true, Delete = true } },
                    { "customers", new RolePermissionDto { Access = true, Edit = true, Delete = false } },
                    { "tickets", new RolePermissionDto { Access = true, Edit = true, Delete = false } },
                    { "reports", new RolePermissionDto { Access = true, Edit = false, Delete = false } },
                    { "settings", new RolePermissionDto { Access = true, Edit = false, Delete = false } }
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new RoleResponseDto
            {
                Id = Guid.NewGuid(),
                Name = "Technician",
                Description = "Repair technician",
                IsSuperUser = false,
                Permissions = new Dictionary<string, RolePermissionDto>
                {
                    { "pos", new RolePermissionDto { Access = false, Edit = false, Delete = false } },
                    { "inventory", new RolePermissionDto { Access = true, Edit = false, Delete = false } },
                    { "customers", new RolePermissionDto { Access = true, Edit = false, Delete = false } },
                    { "tickets", new RolePermissionDto { Access = true, Edit = true, Delete = false } },
                    { "reports", new RolePermissionDto { Access = false, Edit = false, Delete = false } },
                    { "settings", new RolePermissionDto { Access = false, Edit = false, Delete = false } }
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new RoleResponseDto
            {
                Id = Guid.NewGuid(),
                Name = "Cashier",
                Description = "Point of sale cashier",
                IsSuperUser = false,
                Permissions = new Dictionary<string, RolePermissionDto>
                {
                    { "pos", new RolePermissionDto { Access = true, Edit = true, Delete = false } },
                    { "inventory", new RolePermissionDto { Access = true, Edit = false, Delete = false } },
                    { "customers", new RolePermissionDto { Access = true, Edit = true, Delete = false } },
                    { "tickets", new RolePermissionDto { Access = false, Edit = false, Delete = false } },
                    { "reports", new RolePermissionDto { Access = false, Edit = false, Delete = false } },
                    { "settings", new RolePermissionDto { Access = false, Edit = false, Delete = false } }
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        foreach (var role in defaultRoles)
        {
            _roles[role.Id] = role;
            _roleNameToId[role.Name] = role.Id;
        }
    }
}

