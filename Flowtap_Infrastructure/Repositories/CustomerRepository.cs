using Flowtap_Domain.BoundedContexts.Sales.Entities;
using Flowtap_Domain.BoundedContexts.Sales.Interfaces;
using Flowtap_Domain.SharedKernel.Enums;
using Flowtap_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flowtap_Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Customer?> GetByPhoneAsync(string phone)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Phone == phone);
    }

    public async Task<IEnumerable<Customer>> GetByStatusAsync(string status)
    {
        // Convert string to enum for backward compatibility
        if (Enum.TryParse<CustomerStatus>(status, true, out var customerStatus))
        {
            return await _context.Customers
                .Where(c => c.Status == customerStatus)
                .ToListAsync();
        }
        return new List<Customer>();
    }

    public async Task<IEnumerable<Customer>> GetByStoreIdAsync(Guid storeId)
    {
        return await _context.Customers
            .Where(c => c.StoreId == storeId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Customer>> GetByStoreIdsAsync(IEnumerable<Guid> storeIds)
    {
        var storeIdsList = storeIds.ToList();
        if (!storeIdsList.Any())
        {
            return new List<Customer>();
        }

        return await _context.Customers
            .Where(c => storeIdsList.Contains(c.StoreId))
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Customer>> SearchAsync(string searchTerm)
    {
        return await _context.Customers
            .Where(c => c.Name.Contains(searchTerm) ||
                       (c.Email != null && c.Email.Contains(searchTerm)) ||
                       (c.Phone != null && c.Phone.Contains(searchTerm)))
            .ToListAsync();
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        customer.Id = Guid.NewGuid();
        customer.CreatedAt = DateTime.UtcNow;
        customer.UpdatedAt = DateTime.UtcNow;
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer> UpdateAsync(Customer customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            return false;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Customers.AnyAsync(c => c.Id == id);
    }
}

