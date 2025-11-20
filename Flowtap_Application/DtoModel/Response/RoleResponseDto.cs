using Flowtap_Application.DtoModel;

namespace Flowtap_Application.DtoModel.Response;

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

