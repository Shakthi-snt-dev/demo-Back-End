using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.DtoModel;

public class CreateEmployeeRequestDto
{
    [Required]
    public Guid StoreId { get; set; }

    [MaxLength(200)]
    public string? FullName { get; set; }

    [Required, MaxLength(320)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(100)]
    public string? Role { get; set; } // admin, manager, technician, cashier

    [MaxLength(50)]
    public string? EmployeeCode { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? HourlyRate { get; set; }

    public AddressDto? Address { get; set; }

    [MaxLength(200)]
    public string? EmergencyContactName { get; set; }

    [MaxLength(20)]
    public string? EmergencyContactPhone { get; set; }

    public Guid? LinkedAppUserId { get; set; }

    public Dictionary<string, bool>? Permissions { get; set; } // pos, inventory, customers, tickets, reports, settings
}

public class UpdateEmployeeRequestDto
{
    [MaxLength(200)]
    public string? FullName { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(100)]
    public string? Role { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? HourlyRate { get; set; }

    public AddressDto? Address { get; set; }

    [MaxLength(200)]
    public string? EmergencyContactName { get; set; }

    [MaxLength(20)]
    public string? EmergencyContactPhone { get; set; }

    public bool? IsActive { get; set; }

    public Dictionary<string, bool>? Permissions { get; set; }
}

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

/// <summary>
/// DTO for adding a partner/admin to a store
/// Only the owner (AppUser who owns the subscription) can add partners
/// </summary>
public class AddPartnerRequestDto
{
    [Required]
    public Guid StoreId { get; set; }

    [Required, MaxLength(320)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? FullName { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [Required]
    [MaxLength(100)]
    public string Role { get; set; } = string.Empty; // Partner, Admin, SuperAdmin

    public AddressDto? Address { get; set; }

    public Guid? LinkedAppUserId { get; set; } // If linking to existing AppUser account
}

