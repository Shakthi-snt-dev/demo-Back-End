using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.DtoModel;

/// <summary>
/// DTO for creating a business owner (AppUserAdmin)
/// </summary>
public class CreateAppUserAdminRequestDto
{
    [Required]
    public Guid AppUserId { get; set; }

    [MaxLength(200)]
    public string? FullName { get; set; }

    [Required, MaxLength(320)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public bool IsPrimaryOwner { get; set; } = false;

    public AddressDto? Address { get; set; }

    public List<string>? Permissions { get; set; }

    /// <summary>
    /// If true, creates an Employee record with Owner role for this admin
    /// </summary>
    public bool CreateAsEmployee { get; set; } = true;

    /// <summary>
    /// Store ID where the employee should be created (required if CreateAsEmployee is true)
    /// </summary>
    public Guid? StoreId { get; set; }
}

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

