using Flowtap_Domain.BoundedContexts.Service.Entities;

namespace Flowtap_Domain.BoundedContexts.Service.Interfaces;

public interface IDeviceBrandRepository
{
    Task<DeviceBrand?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DeviceBrand>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<DeviceBrand>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<DeviceBrand> CreateAsync(DeviceBrand brand, CancellationToken cancellationToken = default);
    Task UpdateAsync(DeviceBrand brand, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

