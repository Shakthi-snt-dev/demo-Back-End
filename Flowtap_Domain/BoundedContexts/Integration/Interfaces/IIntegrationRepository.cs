using IntegrationEntity = Flowtap_Domain.BoundedContexts.Integration.Entities.Integration;

namespace Flowtap_Domain.BoundedContexts.Integration.Interfaces;

public interface IIntegrationRepository
{
    Task<IntegrationEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<IntegrationEntity>> GetByAppUserIdAsync(Guid appUserId);
    Task<IntegrationEntity?> GetByAppUserIdAndTypeAsync(Guid appUserId, string type);
    Task<IEnumerable<IntegrationEntity>> GetConnectedIntegrationsAsync(Guid appUserId);
    Task<IntegrationEntity> CreateAsync(IntegrationEntity integration);
    Task<IntegrationEntity> UpdateAsync(IntegrationEntity integration);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}

