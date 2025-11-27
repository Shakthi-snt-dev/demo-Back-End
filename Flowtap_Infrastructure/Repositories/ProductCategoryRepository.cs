using Flowtap_Domain.BoundedContexts.Inventory.Entities;
using Flowtap_Domain.BoundedContexts.Inventory.Interfaces;
using Flowtap_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flowtap_Infrastructure.Repositories;

public class ProductCategoryRepository : IProductCategoryRepository
{
    private readonly AppDbContext _context;

    public ProductCategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProductCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategories
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ProductCategory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategories
            .Include(c => c.SubCategories)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductCategory?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategories
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
    }

    public async Task<ProductCategory> CreateAsync(ProductCategory category, CancellationToken cancellationToken = default)
    {
        category.Id = Guid.NewGuid();
        _context.ProductCategories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);
        return category;
    }

    public async Task UpdateAsync(ProductCategory category, CancellationToken cancellationToken = default)
    {
        _context.ProductCategories.Update(category);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _context.ProductCategories.FindAsync(new object[] { id }, cancellationToken);
        if (category == null)
            return false;

        _context.ProductCategories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategories.AnyAsync(c => c.Id == id, cancellationToken);
    }
}

