using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.BoundedContexts.Service.Interfaces;
using Flowtap_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flowtap_Infrastructure.Repositories;

public class SpecialOrderPartRepository : ISpecialOrderPartRepository
{
    private readonly AppDbContext _context;

    public SpecialOrderPartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SpecialOrderPart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SpecialOrderParts.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<SpecialOrderPart>> GetByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default)
    {
        return await _context.SpecialOrderParts
            .Where(sop => sop.StoreId == storeId)
            .OrderByDescending(sop => sop.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SpecialOrderPart>> GetBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken = default)
    {
        return await _context.SpecialOrderParts
            .Where(sop => sop.SupplierId == supplierId)
            .OrderByDescending(sop => sop.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SpecialOrderPart>> GetPendingOrdersAsync(Guid storeId, CancellationToken cancellationToken = default)
    {
        return await _context.SpecialOrderParts
            .Where(sop => sop.StoreId == storeId && sop.ReceivedDate == null)
            .OrderByDescending(sop => sop.OrderDate ?? sop.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<SpecialOrderPart> CreateAsync(SpecialOrderPart part, CancellationToken cancellationToken = default)
    {
        part.Id = Guid.NewGuid();
        part.CreatedAt = DateTime.UtcNow;
        part.UpdatedAt = DateTime.UtcNow;
        _context.SpecialOrderParts.Add(part);
        await _context.SaveChangesAsync(cancellationToken);
        return part;
    }

    public async Task UpdateAsync(SpecialOrderPart part, CancellationToken cancellationToken = default)
    {
        part.UpdatedAt = DateTime.UtcNow;
        _context.SpecialOrderParts.Update(part);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var part = await _context.SpecialOrderParts.FindAsync(new object[] { id }, cancellationToken);
        if (part == null)
            return false;

        _context.SpecialOrderParts.Remove(part);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

