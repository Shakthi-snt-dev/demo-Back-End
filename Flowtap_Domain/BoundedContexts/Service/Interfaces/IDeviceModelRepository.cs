using Flowtap_Domain.BoundedContexts.Service.Entities;

namespace Flowtap_Domain.BoundedContexts.Service.Interfaces;

public interface IDeviceModelRepository
{
    Task<DeviceModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DeviceModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<DeviceModel>> GetByBrandIdAsync(Guid brandId, CancellationToken cancellationToken = default);
    Task<DeviceModel> CreateAsync(DeviceModel model, CancellationToken cancellationToken = default);
    Task UpdateAsync(DeviceModel model, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

