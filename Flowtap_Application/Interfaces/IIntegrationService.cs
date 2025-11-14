using Flowtap_Domain.DtoModel;

namespace Flowtap_Application.Interfaces;

public interface IIntegrationService
{
    Task<IntegrationResponseDto> CreateQuickBooksIntegrationAsync(CreateQuickBooksIntegrationRequestDto request);
    Task<IntegrationResponseDto> CreateShopifyIntegrationAsync(CreateShopifyIntegrationRequestDto request);
    Task<IntegrationResponseDto> GetIntegrationByIdAsync(Guid id);
    Task<IEnumerable<IntegrationResponseDto>> GetIntegrationsByAppUserIdAsync(Guid appUserId);
    Task<IntegrationResponseDto?> GetIntegrationByTypeAsync(Guid appUserId, string type);
    Task<IntegrationResponseDto> UpdateIntegrationAsync(Guid id, UpdateIntegrationRequestDto request);
    Task<IntegrationResponseDto> ConnectIntegrationAsync(Guid id);
    Task<IntegrationResponseDto> DisconnectIntegrationAsync(Guid id);
    Task<IntegrationResponseDto> EnableIntegrationAsync(Guid id);
    Task<IntegrationResponseDto> DisableIntegrationAsync(Guid id);
    Task<SyncResultDto> SyncIntegrationAsync(Guid id, SyncIntegrationRequestDto? request = null);
    Task<bool> DeleteIntegrationAsync(Guid id);
}

