using Flowtap_Domain.BoundedContexts.Service.Entities;

namespace Flowtap_Domain.BoundedContexts.Service.Interfaces;

public interface IDeviceCategoryRepository
{
    Task<DeviceCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DeviceCategory>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DeviceCategory?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<DeviceCategory> CreateAsync(DeviceCategory category, CancellationToken cancellationToken = default);
    Task UpdateAsync(DeviceCategory category, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

