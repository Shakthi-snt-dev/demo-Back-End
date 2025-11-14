using Flowtap_Domain.BoundedContexts.HR.Entities;

namespace Flowtap_Domain.BoundedContexts.HR.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Employee?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Employee?> GetByEmployeeCodeAsync(string employeeCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<Employee>> GetByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Employee>> GetActiveEmployeesByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Employee>> GetByRoleAsync(string role, CancellationToken cancellationToken = default);
    Task<IEnumerable<Employee>> GetByLinkedAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default);
    Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken = default);
    Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> EmployeeCodeExistsAsync(string employeeCode, CancellationToken cancellationToken = default);
}

