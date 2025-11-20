using System.ComponentModel.DataAnnotations;

namespace Flowtap_Application.DtoModel.Request;

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

public class SyncIntegrationRequestDto
{
    public string? SyncType { get; set; } // products, customers, orders, all
}

