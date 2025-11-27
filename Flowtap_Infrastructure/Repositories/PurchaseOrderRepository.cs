using Flowtap_Domain.BoundedContexts.Procurement.Entities;
using Flowtap_Domain.BoundedContexts.Procurement.Interfaces;
using Flowtap_Domain.SharedKernel.Enums;
using Flowtap_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flowtap_Infrastructure.Repositories;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly AppDbContext _context;

    public PurchaseOrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.Lines)
            .FirstOrDefaultAsync(po => po.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<PurchaseOrder>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.Lines)
            .OrderByDescending(po => po.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PurchaseOrder>> GetByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.Lines)
            .Where(po => po.StoreId == storeId)
            .OrderByDescending(po => po.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PurchaseOrder>> GetBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.Lines)
            .Where(po => po.SupplierId == supplierId)
            .OrderByDescending(po => po.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<PurchaseOrder?> GetByPONumberAsync(string poNumber, CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.Lines)
            .FirstOrDefaultAsync(po => po.PONumber == poNumber, cancellationToken);
    }

    public async Task<IEnumerable<PurchaseOrder>> GetByStatusAsync(PurchaseOrderStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.Lines)
            .Where(po => po.Status == status)
            .OrderByDescending(po => po.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<PurchaseOrder> CreateAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default)
    {
        purchaseOrder.Id = Guid.NewGuid();
        purchaseOrder.CreatedAt = DateTime.UtcNow;
        _context.PurchaseOrders.Add(purchaseOrder);
        await _context.SaveChangesAsync(cancellationToken);
        return purchaseOrder;
    }

    public async Task UpdateAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default)
    {
        _context.PurchaseOrders.Update(purchaseOrder);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var purchaseOrder = await _context.PurchaseOrders.FindAsync(new object[] { id }, cancellationToken);
        if (purchaseOrder == null)
            return false;

        _context.PurchaseOrders.Remove(purchaseOrder);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

