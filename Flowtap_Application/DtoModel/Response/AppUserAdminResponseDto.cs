using Flowtap_Application.DtoModel;

namespace Flowtap_Application.DtoModel.Response;

public class AppUserAdminResponseDto
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public string? FullName { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool IsPrimaryOwner { get; set; }
    public AddressDto? Address { get; set; }
    public List<string> Permissions { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public Guid? EmployeeId { get; set; } // If created as employee
}

