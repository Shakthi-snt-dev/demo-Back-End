using Flowtap_Domain.BoundedContexts.Audit.Entities;

namespace Flowtap_Domain.BoundedContexts.Audit.Interfaces;

public interface IActivityLogRepository
{
    Task<ActivityLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ActivityLog>> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ActivityLog>> GetByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ActivityLog>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ActivityLog>> GetByActionAsync(string action, CancellationToken cancellationToken = default);
    Task<IEnumerable<ActivityLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<ActivityLog>> GetRecentLogsAsync(int count, CancellationToken cancellationToken = default);
    Task<ActivityLog> AddAsync(ActivityLog activityLog, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<ActivityLog> activityLogs, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeleteOldLogsAsync(DateTime beforeDate, CancellationToken cancellationToken = default);
}

