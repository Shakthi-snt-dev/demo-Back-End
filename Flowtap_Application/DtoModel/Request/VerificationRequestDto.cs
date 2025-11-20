using System.ComponentModel.DataAnnotations;

namespace Flowtap_Application.DtoModel.Request;

public class VerifyEmailRequestDto
{
    [Required(ErrorMessage = "Token is required")]
    public string Token { get; set; } = string.Empty;
}

public class OnboardingStep1RequestDto
{
    [Required(ErrorMessage = "First name is required")]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; }

    [MaxLength(200)]
    public string? BusinessName { get; set; }

    [MaxLength(100)]
    public string? Country { get; set; }

    [MaxLength(10)]
    public string Currency { get; set; } = "USD";

    [MaxLength(50)]
    public string TimeZone { get; set; } = "UTC";

    // Address fields
    [Required]
    public string StreetNumber { get; set; } = string.Empty;

    [Required]
    public string StreetName { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = string.Empty;

    [Required]
    public string PostalCode { get; set; } = string.Empty;
}

public class OnboardingStep2RequestDto
{
    [Required(ErrorMessage = "Store name is required")]
    [MaxLength(200)]
    public string StoreName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? StoreType { get; set; }

    [MaxLength(100)]
    public string? StoreCategory { get; set; }

    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? Phone { get; set; }

    // Store Address
    [Required]
    public string StreetNumber { get; set; } = string.Empty;

    [Required]
    public string StreetName { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = string.Empty;

    [Required]
    public string PostalCode { get; set; } = string.Empty;

    // Store Settings
    public bool EnablePOS { get; set; } = true;
    public bool EnableInventory { get; set; } = true;
    public string TimeZone { get; set; } = "UTC";
}

public class OnboardingStep3RequestDto
{
    [Required(ErrorMessage = "Action is required")]
    public string Action { get; set; } = string.Empty; // "dive_in" or "demo"
}

