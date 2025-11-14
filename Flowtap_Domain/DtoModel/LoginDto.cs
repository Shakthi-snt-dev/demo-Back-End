using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.DtoModel;

public class LoginRequestDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Token { get; set; }
    public UserInfoDto? User { get; set; }
}

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

