using Microsoft.EntityFrameworkCore;
using Flowtap_Domain.BoundedContexts.Audit.Entities;
using Flowtap_Domain.BoundedContexts.Audit.Interfaces;
using Flowtap_Infrastructure.Data;

namespace Flowtap_Infrastructure.Repositories;

public class ActivityLogRepository : IActivityLogRepository
{
    private readonly AppDbContext _context;

    public ActivityLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ActivityLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ActivityLogs
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ActivityLog>> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default)
    {
        return await _context.ActivityLogs
            .Where(a => a.AppUserId == appUserId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ActivityLog>> GetByStoreIdAsync(Guid storeId, CancellationToken cancellationToken = default)
    {
        return await _context.ActivityLogs
            .Where(a => a.StoreId == storeId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ActivityLog>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        return await _context.ActivityLogs
            .Where(a => a.EmployeeId == employeeId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ActivityLog>> GetByActionAsync(string action, CancellationToken cancellationToken = default)
    {
        return await _context.ActivityLogs
            .Where(a => a.Action == action)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ActivityLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.ActivityLogs
            .Where(a => a.CreatedAt >= startDate && a.CreatedAt <= endDate)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ActivityLog>> GetRecentLogsAsync(int count, CancellationToken cancellationToken = default)
    {
        return await _context.ActivityLogs
            .OrderByDescending(a => a.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<ActivityLog> AddAsync(ActivityLog activityLog, CancellationToken cancellationToken = default)
    {
        await _context.ActivityLogs.AddAsync(activityLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return activityLog;
    }

    public async Task AddRangeAsync(IEnumerable<ActivityLog> activityLogs, CancellationToken cancellationToken = default)
    {
        await _context.ActivityLogs.AddRangeAsync(activityLogs, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var activityLog = await GetByIdAsync(id, cancellationToken);
        if (activityLog != null)
        {
            _context.ActivityLogs.Remove(activityLog);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeleteOldLogsAsync(DateTime beforeDate, CancellationToken cancellationToken = default)
    {
        var oldLogs = await _context.ActivityLogs
            .Where(a => a.CreatedAt < beforeDate)
            .ToListAsync(cancellationToken);

        _context.ActivityLogs.RemoveRange(oldLogs);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

