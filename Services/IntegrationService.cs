using FlowTap.Api.Data;
using FlowTap.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowTap.Api.Services;

public class IntegrationService : IIntegrationService
{
    private readonly ApplicationDbContext _context;

    public IntegrationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Integration> ConnectShopifyAsync(ConnectShopifyRequest request)
    {
        var existing = await _context.Integrations
            .FirstOrDefaultAsync(i => i.Type == IntegrationType.Shopify);

        if (existing != null)
        {
            existing.ApiKey = request.ApiKey;
            existing.Secret = request.Secret;
            existing.Token = request.StoreUrl;
            existing.Status = IntegrationStatus.Connected;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            existing = new Integration
            {
                Type = IntegrationType.Shopify,
                ApiKey = request.ApiKey,
                Secret = request.Secret,
                Token = request.StoreUrl,
                Status = IntegrationStatus.Connected,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Integrations.Add(existing);
        }

        await _context.SaveChangesAsync();
        return existing;
    }

    public Task<bool> SyncShopifyAsync()
    {
        // Hangfire job will handle actual sync
        return Task.FromResult(true);
    }

    public async Task<Integration> ConnectWooCommerceAsync(ConnectWooCommerceRequest request)
    {
        var existing = await _context.Integrations
            .FirstOrDefaultAsync(i => i.Type == IntegrationType.WooCommerce);

        if (existing != null)
        {
            existing.ApiKey = request.ApiKey;
            existing.Secret = request.Secret;
            existing.Token = request.StoreUrl;
            existing.Status = IntegrationStatus.Connected;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            existing = new Integration
            {
                Type = IntegrationType.WooCommerce,
                ApiKey = request.ApiKey,
                Secret = request.Secret,
                Token = request.StoreUrl,
                Status = IntegrationStatus.Connected,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Integrations.Add(existing);
        }

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<Integration> ConnectQuickBooksAsync(ConnectQuickBooksRequest request)
    {
        var existing = await _context.Integrations
            .FirstOrDefaultAsync(i => i.Type == IntegrationType.QuickBooks);

        if (existing != null)
        {
            existing.ApiKey = request.ClientId;
            existing.Secret = request.ClientSecret;
            existing.Token = request.AccessToken;
            existing.Status = IntegrationStatus.Connected;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            existing = new Integration
            {
                Type = IntegrationType.QuickBooks,
                ApiKey = request.ClientId,
                Secret = request.ClientSecret,
                Token = request.AccessToken,
                Status = IntegrationStatus.Connected,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Integrations.Add(existing);
        }

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<List<IntegrationDto>> GetIntegrationStatusAsync()
    {
        var integrations = await _context.Integrations.ToListAsync();

        return integrations.Select(i => new IntegrationDto
        {
            Id = i.Id,
            Type = i.Type.ToString(),
            Status = i.Status.ToString(),
            LastSync = i.LastSync,
            ErrorLog = i.ErrorLog
        }).ToList();
    }

    public Task<bool> ResetApiKeyAsync()
    {
        // Generate new API key logic here
        return Task.FromResult(true);
    }
}

