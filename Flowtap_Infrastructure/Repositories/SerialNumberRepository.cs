using Flowtap_Domain.BoundedContexts.Inventory.Entities;
using Flowtap_Domain.BoundedContexts.Inventory.Interfaces;
using Flowtap_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flowtap_Infrastructure.Repositories;

public class SerialNumberRepository : ISerialNumberRepository
{
    private readonly AppDbContext _context;

    public SerialNumberRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SerialNumber?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SerialNumbers
            .Include(s => s.InventoryItem)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<SerialNumber>> GetByInventoryItemIdAsync(Guid inventoryItemId, CancellationToken cancellationToken = default)
    {
        return await _context.SerialNumbers
            .Where(s => s.InventoryItemId == inventoryItemId)
            .ToListAsync(cancellationToken);
    }

    public async Task<SerialNumber?> GetBySerialAsync(string serial, CancellationToken cancellationToken = default)
    {
        return await _context.SerialNumbers
            .Include(s => s.InventoryItem)
            .FirstOrDefaultAsync(s => s.Serial == serial, cancellationToken);
    }

    public async Task<IEnumerable<SerialNumber>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.SerialNumbers
            .Where(s => s.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<SerialNumber> CreateAsync(SerialNumber serialNumber, CancellationToken cancellationToken = default)
    {
        serialNumber.Id = Guid.NewGuid();
        serialNumber.CreatedAt = DateTime.UtcNow;
        _context.SerialNumbers.Add(serialNumber);
        await _context.SaveChangesAsync(cancellationToken);
        return serialNumber;
    }

    public async Task UpdateAsync(SerialNumber serialNumber, CancellationToken cancellationToken = default)
    {
        _context.SerialNumbers.Update(serialNumber);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var serial = await _context.SerialNumbers.FindAsync(new object[] { id }, cancellationToken);
        if (serial == null)
            return false;

        _context.SerialNumbers.Remove(serial);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SerialNumbers.AnyAsync(s => s.Id == id, cancellationToken);
    }
}

