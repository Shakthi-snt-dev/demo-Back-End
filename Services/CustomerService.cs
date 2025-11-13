using FlowTap.Api.Data;
using FlowTap.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowTap.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly ApplicationDbContext _context;

    public CustomerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CustomerDto>> GetCustomersAsync(string? search)
    {
        var query = _context.Customers.AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(c => c.Name.Contains(search) || 
                                     (c.Email != null && c.Email.Contains(search)) ||
                                     (c.Phone != null && c.Phone.Contains(search)));

        var customers = await query.OrderBy(c => c.Name).ToListAsync();

        return customers.Select(c => new CustomerDto
        {
            Id = c.Id,
            Name = c.Name,
            Phone = c.Phone,
            Email = c.Email,
            StoreId = c.StoreId,
            LoyaltyPoints = c.LoyaltyPoints,
            TotalSpent = c.TotalSpent,
            CreatedAt = c.CreatedAt
        }).ToList();
    }

    public async Task<Customer> CreateCustomerAsync(CreateCustomerRequest request)
    {
        var customer = new Customer
        {
            Name = request.Name,
            Phone = request.Phone,
            Email = request.Email,
            StoreId = request.StoreId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task<Customer> UpdateCustomerAsync(Guid id, UpdateCustomerRequest request)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            throw new Exception("Customer not found");

        if (!string.IsNullOrEmpty(request.Name))
            customer.Name = request.Name;
        if (request.Phone != null)
            customer.Phone = request.Phone;
        if (request.Email != null)
            customer.Email = request.Email;

        customer.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task<List<CustomerHistoryDto>> GetCustomerHistoryAsync(Guid customerId)
    {
        var history = await _context.CustomerHistories
            .Where(h => h.CustomerId == customerId)
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync();

        return history.Select(h => new CustomerHistoryDto
        {
            Id = h.Id,
            ReferenceType = h.ReferenceType.ToString(),
            ReferenceId = h.ReferenceId,
            Description = h.Description,
            CreatedAt = h.CreatedAt
        }).ToList();
    }
}

