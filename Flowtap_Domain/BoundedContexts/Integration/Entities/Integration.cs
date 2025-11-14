using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flowtap_Domain.BoundedContexts.Integration.Entities;

public class Integration
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid AppUserId { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Type { get; set; } = string.Empty; // quickbooks, shopify

    public bool Enabled { get; set; } = false;

    public bool Connected { get; set; } = false;

    public DateTime? ConnectedAt { get; set; }

    public DateTime? LastSyncAt { get; set; }

    // JSON column for settings
    [Column(TypeName = "text")]
    public string SettingsJson { get; set; } = "{}";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Domain methods
    public void Connect()
    {
        Connected = true;
        ConnectedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Disconnect()
    {
        Connected = false;
        ConnectedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Enable()
    {
        Enabled = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Disable()
    {
        Enabled = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateLastSync()
    {
        LastSyncAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}

