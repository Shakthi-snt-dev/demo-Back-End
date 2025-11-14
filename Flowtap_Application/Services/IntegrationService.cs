using Flowtap_Application.Interfaces;
using Flowtap_Domain.BoundedContexts.Integration.Entities;
using Flowtap_Domain.BoundedContexts.Integration.Interfaces;
using Flowtap_Domain.DtoModel;
using Flowtap_Domain.Exceptions;
using System.Text.Json;

namespace Flowtap_Application.Services;

public class IntegrationService : IIntegrationService
{
    private readonly IIntegrationRepository _integrationRepository;
    private readonly ILogger<IntegrationService> _logger;

    public IntegrationService(
        IIntegrationRepository integrationRepository,
        ILogger<IntegrationService> logger)
    {
        _integrationRepository = integrationRepository;
        _logger = logger;
    }

    public async Task<IntegrationResponseDto> CreateQuickBooksIntegrationAsync(CreateQuickBooksIntegrationRequestDto request)
    {
        // Check if integration already exists
        var existing = await _integrationRepository.GetByAppUserIdAndTypeAsync(request.AppUserId, "quickbooks");
        if (existing != null)
            throw new Flowtap_Domain.Exceptions.InvalidOperationException(
                "QuickBooks integration already exists for this user",
                "Integration",
                new Dictionary<string, string> { { "AppUserId", request.AppUserId.ToString() }, { "Type", "quickbooks" } });

        var settings = new Dictionary<string, object>
        {
            { "clientId", request.ClientId },
            { "clientSecret", request.ClientSecret },
            { "environment", request.Environment },
            { "syncProducts", request.SyncProducts },
            { "syncCustomers", request.SyncCustomers },
            { "syncInvoices", request.SyncInvoices },
            { "syncPayments", request.SyncPayments },
            { "autoSync", request.AutoSync },
            { "syncInterval", request.SyncInterval }
        };

        var integration = new Integration
        {
            Id = Guid.NewGuid(),
            AppUserId = request.AppUserId,
            Name = "QuickBooks",
            Type = "quickbooks",
            Enabled = false,
            Connected = false,
            SettingsJson = JsonSerializer.Serialize(settings),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _integrationRepository.CreateAsync(integration);
        return MapToDto(created);
    }

    public async Task<IntegrationResponseDto> CreateShopifyIntegrationAsync(CreateShopifyIntegrationRequestDto request)
    {
        // Check if integration already exists
        var existing = await _integrationRepository.GetByAppUserIdAndTypeAsync(request.AppUserId, "shopify");
        if (existing != null)
            throw new Flowtap_Domain.Exceptions.InvalidOperationException(
                "Shopify integration already exists for this user",
                "Integration",
                new Dictionary<string, string> { { "AppUserId", request.AppUserId.ToString() }, { "Type", "shopify" } });

        var settings = new Dictionary<string, object>
        {
            { "shopDomain", request.ShopDomain },
            { "apiKey", request.ApiKey },
            { "apiSecret", request.ApiSecret },
            { "syncProducts", request.SyncProducts },
            { "syncOrders", request.SyncOrders },
            { "syncCustomers", request.SyncCustomers },
            { "syncInventory", request.SyncInventory },
            { "autoSync", request.AutoSync },
            { "syncInterval", request.SyncInterval }
        };

        var integration = new Integration
        {
            Id = Guid.NewGuid(),
            AppUserId = request.AppUserId,
            Name = "Shopify",
            Type = "shopify",
            Enabled = false,
            Connected = false,
            SettingsJson = JsonSerializer.Serialize(settings),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _integrationRepository.CreateAsync(integration);
        return MapToDto(created);
    }

    public async Task<IntegrationResponseDto> GetIntegrationByIdAsync(Guid id)
    {
        var integration = await _integrationRepository.GetByIdAsync(id);
        if (integration == null)
            throw new EntityNotFoundException("Integration", id);

        return MapToDto(integration);
    }

    public async Task<IEnumerable<IntegrationResponseDto>> GetIntegrationsByAppUserIdAsync(Guid appUserId)
    {
        var integrations = await _integrationRepository.GetByAppUserIdAsync(appUserId);
        return integrations.Select(MapToDto);
    }

    public async Task<IntegrationResponseDto?> GetIntegrationByTypeAsync(Guid appUserId, string type)
    {
        var integration = await _integrationRepository.GetByAppUserIdAndTypeAsync(appUserId, type);
        return integration != null ? MapToDto(integration) : null;
    }

    public async Task<IntegrationResponseDto> UpdateIntegrationAsync(Guid id, UpdateIntegrationRequestDto request)
    {
        var integration = await _integrationRepository.GetByIdAsync(id);
        if (integration == null)
            throw new EntityNotFoundException("Integration", id);

        if (request.Enabled.HasValue)
        {
            if (request.Enabled.Value)
                integration.Enable();
            else
                integration.Disable();
        }

        if (request.Settings != null)
        {
            var currentSettings = JsonSerializer.Deserialize<Dictionary<string, object>>(integration.SettingsJson) ?? new Dictionary<string, object>();
            foreach (var setting in request.Settings)
            {
                currentSettings[setting.Key] = setting.Value;
            }
            integration.SettingsJson = JsonSerializer.Serialize(currentSettings);
        }

        var updated = await _integrationRepository.UpdateAsync(integration);
        return MapToDto(updated);
    }

    public async Task<IntegrationResponseDto> ConnectIntegrationAsync(Guid id)
    {
        var integration = await _integrationRepository.GetByIdAsync(id);
        if (integration == null)
            throw new EntityNotFoundException("Integration", id);

        integration.Connect();
        var updated = await _integrationRepository.UpdateAsync(integration);
        return MapToDto(updated);
    }

    public async Task<IntegrationResponseDto> DisconnectIntegrationAsync(Guid id)
    {
        var integration = await _integrationRepository.GetByIdAsync(id);
        if (integration == null)
            throw new EntityNotFoundException("Integration", id);

        integration.Disconnect();
        var updated = await _integrationRepository.UpdateAsync(integration);
        return MapToDto(updated);
    }

    public async Task<IntegrationResponseDto> EnableIntegrationAsync(Guid id)
    {
        var integration = await _integrationRepository.GetByIdAsync(id);
        if (integration == null)
            throw new EntityNotFoundException("Integration", id);

        integration.Enable();
        var updated = await _integrationRepository.UpdateAsync(integration);
        return MapToDto(updated);
    }

    public async Task<IntegrationResponseDto> DisableIntegrationAsync(Guid id)
    {
        var integration = await _integrationRepository.GetByIdAsync(id);
        if (integration == null)
            throw new EntityNotFoundException("Integration", id);

        integration.Disable();
        var updated = await _integrationRepository.UpdateAsync(integration);
        return MapToDto(updated);
    }

    public async Task<SyncResultDto> SyncIntegrationAsync(Guid id, SyncIntegrationRequestDto? request = null)
    {
        var integration = await _integrationRepository.GetByIdAsync(id);
        if (integration == null)
            throw new EntityNotFoundException("Integration", id);

        if (!integration.Connected)
            throw new Flowtap_Domain.Exceptions.InvalidOperationException(
                "Integration must be connected before syncing",
                "Integration",
                new Dictionary<string, string> { { "IntegrationId", id.ToString() } });

        // TODO: Implement actual sync logic based on integration type
        // This would call QuickBooks/Shopify APIs to sync data
        
        integration.UpdateLastSync();
        await _integrationRepository.UpdateAsync(integration);

        return new SyncResultDto
        {
            Success = true,
            SyncedItems = 0, // Would be actual count after sync
            Errors = new List<string>(),
            Timestamp = DateTime.UtcNow
        };
    }

    public async Task<bool> DeleteIntegrationAsync(Guid id)
    {
        return await _integrationRepository.DeleteAsync(id);
    }

    private static IntegrationResponseDto MapToDto(Integration integration)
    {
        var settings = new Dictionary<string, object>();
        try
        {
            settings = JsonSerializer.Deserialize<Dictionary<string, object>>(integration.SettingsJson) ?? new Dictionary<string, object>();
        }
        catch
        {
            // If deserialization fails, use empty dictionary
        }

        return new IntegrationResponseDto
        {
            Id = integration.Id,
            AppUserId = integration.AppUserId,
            Name = integration.Name,
            Type = integration.Type,
            Enabled = integration.Enabled,
            Connected = integration.Connected,
            ConnectedAt = integration.ConnectedAt,
            LastSyncAt = integration.LastSyncAt,
            Settings = settings,
            CreatedAt = integration.CreatedAt,
            UpdatedAt = integration.UpdatedAt
        };
    }
}

