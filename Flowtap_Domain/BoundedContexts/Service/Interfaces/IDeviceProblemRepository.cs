using Flowtap_Domain.BoundedContexts.Service.Entities;

namespace Flowtap_Domain.BoundedContexts.Service.Interfaces;

public interface IDeviceProblemRepository
{
    Task<DeviceProblem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DeviceProblem>> GetByModelIdAsync(Guid modelId, CancellationToken cancellationToken = default);
    Task<DeviceProblem> CreateAsync(DeviceProblem problem, CancellationToken cancellationToken = default);
    Task UpdateAsync(DeviceProblem problem, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

