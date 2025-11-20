using System.ComponentModel.DataAnnotations;
using Flowtap_Application.DtoModel;

namespace Flowtap_Application.DtoModel.Request;

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

