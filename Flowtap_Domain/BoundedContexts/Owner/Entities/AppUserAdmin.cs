using System.ComponentModel.DataAnnotations;
using Flowtap_Domain.SharedKernel.ValueObjects;

namespace Flowtap_Domain.BoundedContexts.Owner.Entities;

public class AppUserAdmin
{
    [Key]
    public Guid Id { get; set; }

    public Guid AppUserId { get; set; }

    public string? FullName { get; set; }

    [Required, MaxLength(320)]
    public string Email { get; set; } = string.Empty;

    public string? PasswordHash { get; set; }

    public bool IsPrimaryOwner { get; set; }

    public Address? Address { get; set; }

    public ICollection<string> Permissions { get; set; } = new List<string>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

