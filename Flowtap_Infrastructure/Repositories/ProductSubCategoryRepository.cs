using Flowtap_Domain.BoundedContexts.Inventory.Entities;
using Flowtap_Domain.BoundedContexts.Inventory.Interfaces;
using Flowtap_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flowtap_Infrastructure.Repositories;

public class ProductSubCategoryRepository : IProductSubCategoryRepository
{
    private readonly AppDbContext _context;

    public ProductSubCategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProductSubCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProductSubCategories
            .Include(sc => sc.Category)
            .FirstOrDefaultAsync(sc => sc.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ProductSubCategory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProductSubCategories
            .Include(sc => sc.Category)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductSubCategory>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductSubCategories
            .Include(sc => sc.Category)
            .Where(sc => sc.CategoryId == categoryId)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductSubCategory> CreateAsync(ProductSubCategory subCategory, CancellationToken cancellationToken = default)
    {
        subCategory.Id = Guid.NewGuid();
        _context.ProductSubCategories.Add(subCategory);
        await _context.SaveChangesAsync(cancellationToken);
        return subCategory;
    }

    public async Task UpdateAsync(ProductSubCategory subCategory, CancellationToken cancellationToken = default)
    {
        _context.ProductSubCategories.Update(subCategory);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var subCategory = await _context.ProductSubCategories.FindAsync(new object[] { id }, cancellationToken);
        if (subCategory == null)
            return false;

        _context.ProductSubCategories.Remove(subCategory);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProductSubCategories.AnyAsync(sc => sc.Id == id, cancellationToken);
    }
}

