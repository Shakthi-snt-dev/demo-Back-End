using Flowtap_Domain.BoundedContexts.Inventory.Entities;

namespace Flowtap_Domain.BoundedContexts.Inventory.Interfaces;

public interface IProductSubCategoryRepository
{
    Task<ProductSubCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductSubCategory>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductSubCategory>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<ProductSubCategory> CreateAsync(ProductSubCategory subCategory, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProductSubCategory subCategory, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}

