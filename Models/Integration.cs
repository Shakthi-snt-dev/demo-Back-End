namespace FlowTap.Api.Models;

public enum IntegrationType
{
    Shopify,
    WooCommerce,
    QuickBooks,
    Email,
    SMS
}

public enum IntegrationStatus
{
    Connected,
    Disconnected,
    Error
}

public class Integration
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public IntegrationType Type { get; set; }
    public string? ApiKey { get; set; }
    public string? Secret { get; set; }
    public string? Token { get; set; }
    public DateTime? LastSync { get; set; }
    public IntegrationStatus Status { get; set; } = IntegrationStatus.Disconnected;
    public string? ErrorLog { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

