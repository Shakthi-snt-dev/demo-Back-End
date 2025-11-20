namespace Flowtap_Application.DtoModel;

/// <summary>
/// Shared Address DTO used in both Request and Response models
/// </summary>
public class AddressDto
{
    public string StreetNumber { get; set; } = string.Empty;
    public string StreetName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}

/// <summary>
/// Shared User Info DTO
/// </summary>
public class UserInfoDto
{
    public Guid UserId { get; set; }
    public Guid? AppUserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string UserType { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; }
    public DateTime? LastLoginAt { get; set; }
}

/// <summary>
/// Shared Role Permission DTO
/// </summary>
public class RolePermissionDto
{
    public bool Access { get; set; } = false;
    public bool Edit { get; set; } = false;
    public bool Delete { get; set; } = false;
}

