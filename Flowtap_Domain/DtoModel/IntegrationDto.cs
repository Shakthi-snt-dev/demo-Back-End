using System.ComponentModel.DataAnnotations;

namespace Flowtap_Domain.DtoModel;

public class CreateQuickBooksIntegrationRequestDto
{
    [Required]
    public Guid AppUserId { get; set; }

    [Required, MaxLength(200)]
    public string ClientId { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string ClientSecret { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Environment { get; set; } = "sandbox"; // sandbox, production

    public bool SyncProducts { get; set; } = true;
    public bool SyncCustomers { get; set; } = true;
    public bool SyncInvoices { get; set; } = true;
    public bool SyncPayments { get; set; } = true;
    public bool AutoSync { get; set; } = false;
    public int SyncInterval { get; set; } = 60; // minutes
}

public class CreateShopifyIntegrationRequestDto
{
    [Required]
    public Guid AppUserId { get; set; }

    [Required, MaxLength(200)]
    public string ShopDomain { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string ApiKey { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string ApiSecret { get; set; } = string.Empty;

    public bool SyncProducts { get; set; } = true;
    public bool SyncOrders { get; set; } = true;
    public bool SyncCustomers { get; set; } = true;
    public bool SyncInventory { get; set; } = true;
    public bool AutoSync { get; set; } = false;
    public int SyncInterval { get; set; } = 60; // minutes
}

public class UpdateIntegrationRequestDto
{
    public bool? Enabled { get; set; }
    public Dictionary<string, object>? Settings { get; set; }
}

public class IntegrationResponseDto
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public bool Connected { get; set; }
    public DateTime? ConnectedAt { get; set; }
    public DateTime? LastSyncAt { get; set; }
    public Dictionary<string, object> Settings { get; set; } = new Dictionary<string, object>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class SyncIntegrationRequestDto
{
    public string? SyncType { get; set; } // products, customers, orders, all
}

public class SyncResultDto
{
    public bool Success { get; set; }
    public int SyncedItems { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
    public DateTime Timestamp { get; set; }
}

