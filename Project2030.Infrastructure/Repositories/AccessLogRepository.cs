using Microsoft.EntityFrameworkCore;
using Project2030.Application.DTOs.AuditLog;
using Project2030.Application.Interfaces.Repositories;
using Project2030.Domain.Entities;
using Project2030.Infrastructure.Data;

namespace Project2030.Infrastructure.Repositories;

public class AccessLogRepository : IAccessLogRepository
{
    private readonly AppDbContext _context;

    public AccessLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AccessLog log)
    {
        _context.AccessLogs.Add(log);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AccessLog>> GetByFilterAsync(string? userId, DateTime? from, DateTime? to, string? action)
    {
        IQueryable<AccessLog> query = _context.AccessLogs.AsQueryable();

        if (!string.IsNullOrEmpty(userId))
            query = query.Where(l => l.UserId == userId);

        if (from.HasValue)
            query = query.Where(l => l.AccessTime >= from.Value);

        if (to.HasValue)
            query = query.Where(l => l.AccessTime <= to.Value);

        if (!string.IsNullOrEmpty(action))
            query = query.Where(l => l.Action == action.ToUpper());

        return await query.OrderByDescending(l => l.AccessTime).ToListAsync();
    }

    public async Task<(IEnumerable<AccessLog> Items, int TotalCount)> GetPagedAsync(AccessLogFilterDto filter)
    {
        IQueryable<AccessLog> query = _context.AccessLogs
            .Include(a => a.User)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter.UserId))
            query = query.Where(a => a.UserId == filter.UserId);

        if (filter.From.HasValue)
            query = query.Where(a => a.AccessTime >= filter.From.Value);

        if (filter.To.HasValue)
            query = query.Where(a => a.AccessTime <= filter.To.Value);

        if (!string.IsNullOrEmpty(filter.Action))
            query = query.Where(a => a.Action == filter.Action.ToUpper());

        int totalCount = await query.CountAsync();

        List<AccessLog> items = await query
            .OrderByDescending(a => a.AccessTime)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
