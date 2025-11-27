using Flowtap_Domain.BoundedContexts.Service.Entities;

namespace Flowtap_Domain.BoundedContexts.Service.Interfaces;

public interface IDeviceVariantRepository
{
    Task<DeviceVariant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DeviceVariant>> GetByModelIdAsync(Guid modelId, CancellationToken cancellationToken = default);
    Task<DeviceVariant> CreateAsync(DeviceVariant variant, CancellationToken cancellationToken = default);
    Task UpdateAsync(DeviceVariant variant, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

