using Flowtap_Domain.BoundedContexts.Integration.Entities;

namespace Flowtap_Domain.BoundedContexts.Integration.Interfaces;

public interface IIntegrationRepository
{
    Task<Integration?> GetByIdAsync(Guid id);
    Task<IEnumerable<Integration>> GetByAppUserIdAsync(Guid appUserId);
    Task<Integration?> GetByAppUserIdAndTypeAsync(Guid appUserId, string type);
    Task<IEnumerable<Integration>> GetConnectedIntegrationsAsync(Guid appUserId);
    Task<Integration> CreateAsync(Integration integration);
    Task<Integration> UpdateAsync(Integration integration);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}

