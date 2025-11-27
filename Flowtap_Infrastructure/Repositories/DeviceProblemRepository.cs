using Flowtap_Domain.BoundedContexts.Service.Entities;
using Flowtap_Domain.BoundedContexts.Service.Interfaces;
using Flowtap_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flowtap_Infrastructure.Repositories;

public class DeviceProblemRepository : IDeviceProblemRepository
{
    private readonly AppDbContext _context;

    public DeviceProblemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DeviceProblem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DeviceProblems
            .Include(dp => dp.Model)
            .ThenInclude(dm => dm.Brand)
            .FirstOrDefaultAsync(dp => dp.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<DeviceProblem>> GetByModelIdAsync(Guid modelId, CancellationToken cancellationToken = default)
    {
        return await _context.DeviceProblems
            .Include(dp => dp.Model)
            .Where(dp => dp.ModelId == modelId)
            .OrderBy(dp => dp.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<DeviceProblem> CreateAsync(DeviceProblem problem, CancellationToken cancellationToken = default)
    {
        problem.Id = Guid.NewGuid();
        _context.DeviceProblems.Add(problem);
        await _context.SaveChangesAsync(cancellationToken);
        return problem;
    }

    public async Task UpdateAsync(DeviceProblem problem, CancellationToken cancellationToken = default)
    {
        _context.DeviceProblems.Update(problem);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var problem = await _context.DeviceProblems.FindAsync(new object[] { id }, cancellationToken);
        if (problem == null)
            return false;

        _context.DeviceProblems.Remove(problem);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

