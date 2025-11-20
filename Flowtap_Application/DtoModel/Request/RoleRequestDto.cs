using System.ComponentModel.DataAnnotations;
using Flowtap_Application.DtoModel;

namespace Flowtap_Application.DtoModel.Request;

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

