using Project2030.Application.DTOs.AuditLog;
using Project2030.Application.DTOs.Common;
using Project2030.Application.Interfaces;
using Project2030.Application.Interfaces.Repositories;
using Project2030.Domain.Entities;

namespace Project2030.Application.Services;

public class AccessLogService : IAccessLogService
{
    private readonly IAccessLogRepository _accessLogRepository;
    private readonly IGeolocationService _geolocationService;

    public AccessLogService(IAccessLogRepository accessLogRepository, IGeolocationService geolocationService)
    {
        _accessLogRepository = accessLogRepository;
        _geolocationService = geolocationService;
    }

    public async Task RegisterLoginAsync(string userId, string ipAddress, bool success, string? failureReason = null)
    {
        string location = await _geolocationService.ResolveLocationAsync(ipAddress);

        await _accessLogRepository.AddAsync(new AccessLog
        {
            UserId = userId,
            IpAddress = ipAddress,
            Location = location,
            Action = "LOGIN",
            Success = success,
            FailureReason = failureReason,
            AccessTime = DateTime.UtcNow
        });
    }

    public async Task RegisterLogoutAsync(string userId, string ipAddress)
    {
        string location = await _geolocationService.ResolveLocationAsync(ipAddress);

        await _accessLogRepository.AddAsync(new AccessLog
        {
            UserId = userId,
            IpAddress = ipAddress,
            Location = location,
            Action = "LOGOUT",
            Success = true,
            AccessTime = DateTime.UtcNow
        });
    }

    public async Task<IEnumerable<AccessLog>> GetLogsAsync(string? userId, DateTime? from, DateTime? to, string? action)
    {
        return await _accessLogRepository.GetByFilterAsync(userId, from, to, action);
    }

    public async Task<PagedResultDto<AccessLogResponseDto>> GetLogsPagedAsync(AccessLogFilterDto filter)
    {
        int pageSize = Math.Min(filter.PageSize, 100);
        int page = Math.Max(filter.Page, 1);

        AccessLogFilterDto normalizedFilter = new AccessLogFilterDto
        {
            UserId = filter.UserId,
            From = filter.From,
            To = filter.To,
            Action = filter.Action,
            Page = page,
            PageSize = pageSize
        };

        (IEnumerable<AccessLog> items, int totalCount) = await _accessLogRepository.GetPagedAsync(normalizedFilter);

        IEnumerable<AccessLogResponseDto> dtos = items.Select(a => new AccessLogResponseDto
        {
            Id = a.Id,
            UserId = a.UserId,
            UserEmail = a.User.Email ?? string.Empty,
            UserFullName = a.User.FullName,
            AccessTime = a.AccessTime,
            IpAddress = a.IpAddress,
            Location = a.Location,
            Action = a.Action,
            Success = a.Success,
            FailureReason = a.FailureReason
        });

        return new PagedResultDto<AccessLogResponseDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
}
