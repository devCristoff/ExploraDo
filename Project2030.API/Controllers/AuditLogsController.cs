using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project2030.Application.DTOs.AuditLog;
using Project2030.Application.DTOs.Common;
using Project2030.Application.Interfaces;

namespace Project2030.API.Controllers;

[ApiController]
[Route("api/audit-logs")]
[Authorize(Roles = "SuperAdministrador,Administrador")]
public class AuditLogsController : ControllerBase
{
    private readonly IAccessLogService _accessLogService;

    public AuditLogsController(IAccessLogService accessLogService)
    {
        _accessLogService = accessLogService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<AccessLogResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetLogs([FromQuery] AccessLogFilterDto filter)
    {
        PagedResultDto<AccessLogResponseDto> result = await _accessLogService.GetLogsPagedAsync(filter);
        return Ok(result);
    }
}
