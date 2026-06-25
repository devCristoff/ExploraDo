using Project2030.Application.DTOs.AuditLog;
using Project2030.Domain.Entities;

namespace Project2030.Application.Interfaces.Repositories;

public interface IAccessLogRepository
{
    Task AddAsync(AccessLog log);
    Task<IEnumerable<AccessLog>> GetByFilterAsync(string? userId, DateTime? from, DateTime? to, string? action);
    Task<(IEnumerable<AccessLog> Items, int TotalCount)> GetPagedAsync(AccessLogFilterDto filter);
}
