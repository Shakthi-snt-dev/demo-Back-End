using Flowtap_Application.DtoModel;

namespace Flowtap_Application.DtoModel.Response;

public class EmployeeResponseDto
{
    public Guid Id { get; set; }
    public Guid StoreId { get; set; }
    public string? FullName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Role { get; set; }
    public string? EmployeeCode { get; set; }
    public decimal? HourlyRate { get; set; }
    public AddressDto? Address { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public Guid? LinkedAppUserId { get; set; }
    public bool IsActive { get; set; }
    public Dictionary<string, bool>? Permissions { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

