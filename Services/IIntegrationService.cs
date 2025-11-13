using FlowTap.Api.Models;

namespace FlowTap.Api.Services;

public interface IIntegrationService
{
    Task<Integration> ConnectShopifyAsync(ConnectShopifyRequest request);
    Task<bool> SyncShopifyAsync();
    Task<Integration> ConnectWooCommerceAsync(ConnectWooCommerceRequest request);
    Task<Integration> ConnectQuickBooksAsync(ConnectQuickBooksRequest request);
    Task<List<IntegrationDto>> GetIntegrationStatusAsync();
    Task<bool> ResetApiKeyAsync();
}

public class ConnectShopifyRequest
{
    public string ApiKey { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public string StoreUrl { get; set; } = string.Empty;
}

public class ConnectWooCommerceRequest
{
    public string ApiKey { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public string StoreUrl { get; set; } = string.Empty;
}

public class ConnectQuickBooksRequest
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
}

public class IntegrationDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? LastSync { get; set; }
    public string? ErrorLog { get; set; }
}

