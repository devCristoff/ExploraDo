using Project2030.Application.DTOs.AuditLog;
using Project2030.Application.DTOs.Common;
using Project2030.Domain.Entities;

namespace Project2030.Application.Interfaces;

/// <summary>
/// Contrato para el registro y consulta de logs de auditoría de accesos.
/// </summary>
public interface IAccessLogService
{
    /// <summary>
    /// Registra un intento de inicio de sesión (exitoso o fallido) en la auditoría.
    /// </summary>
    Task RegisterLoginAsync(string userId, string ipAddress, bool success, string? failureReason = null);

    /// <summary>
    /// Registra un cierre de sesión en la auditoría.
    /// </summary>
    Task RegisterLogoutAsync(string userId, string ipAddress);

    /// <summary>
    /// Consulta los registros de auditoría con filtros opcionales (sin paginación).
    /// </summary>
    Task<IEnumerable<AccessLog>> GetLogsAsync(string? userId, DateTime? from, DateTime? to, string? action);

    /// <summary>
    /// Consulta los registros de auditoría con filtros y paginación.
    /// </summary>
    Task<PagedResultDto<AccessLogResponseDto>> GetLogsPagedAsync(AccessLogFilterDto filter);
}
