using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.DtoModel;

public class CreateRoleRequestDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsSuperUser { get; set; } = false;

    public Dictionary<string, RolePermissionDto>? Permissions { get; set; }
}

public class UpdateRoleRequestDto
{
    [MaxLength(500)]
    public string? Description { get; set; }

    public bool? IsSuperUser { get; set; }

    public Dictionary<string, RolePermissionDto>? Permissions { get; set; }
}

public class RolePermissionDto
{
    public bool Access { get; set; } = false;
    public bool Edit { get; set; } = false;
    public bool Delete { get; set; } = false;
}

public class RoleResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsSuperUser { get; set; }
    public Dictionary<string, RolePermissionDto> Permissions { get; set; } = new();
    public int EmployeeCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

