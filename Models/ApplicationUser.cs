using Microsoft.AspNetCore.Identity;

namespace FlowTap.Api.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public string Username { get; set; } = string.Empty;
    public Guid? RoleId { get; set; }
    public ApplicationRole? Role { get; set; }
    public new bool TwoFactorEnabled { get; set; }
    public string? AccessPin { get; set; }
    public string? ProfilePicture { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

