using Microsoft.EntityFrameworkCore;
using Flowtap_Domain.BoundedContexts.HR.Entities;
using Flowtap_Domain.BoundedContexts.HR.Interfaces;
using Flowtap_Infrastructure.Data;

namespace Flowtap_Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<Employee?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(e => e.Email == email, cancellationToken);
    }

    public async Task<Employee?> GetByEmployeeCodeAsync(string employeeCode, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode, cancellationToken);
    }

    public async Task<IEnumerable<Employee>> GetByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .Where(e => e.StoreId == storeId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Employee>> GetActiveEmployeesByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .Where(e => e.StoreId == storeId && e.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Employee>> GetByRoleAsync(string role, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .Where(e => e.Role == role)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Employee>> GetByLinkedAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .Where(e => e.LinkedAppUserId == appUserId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        await _context.Employees.AddAsync(employee, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return employee;
    }

    public async Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var employee = await GetByIdAsync(id, cancellationToken);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .AnyAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .AnyAsync(e => e.Email == email, cancellationToken);
    }

    public async Task<bool> EmployeeCodeExistsAsync(string employeeCode, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .AnyAsync(e => e.EmployeeCode == employeeCode, cancellationToken);
    }
}

