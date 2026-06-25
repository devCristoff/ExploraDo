using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project2030.Application.DTOs.Permissions;
using Project2030.Application.Interfaces;

namespace Project2030.API.Controllers;

[ApiController]
[Route("api/permissions")]
[Authorize]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionsController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdministrador")]
    [ProducesResponseType(typeof(PermissionResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignPermission([FromBody] AssignPermissionRequestDto request)
    {
        PermissionResponseDto permission = await _permissionService.AssignPermissionAsync(request);
        return CreatedAtAction(nameof(GetPermissionsByRole), new { roleId = permission.RoleId }, permission);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdministrador")]
    [ProducesResponseType(typeof(PermissionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePermission(int id, [FromBody] AssignPermissionRequestDto request)
    {
        PermissionResponseDto permission = await _permissionService.UpdatePermissionAsync(id, request);
        return Ok(permission);
    }

    [HttpGet("role/{roleId}")]
    [Authorize(Roles = "SuperAdministrador,Administrador")]
    [ProducesResponseType(typeof(IEnumerable<PermissionResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPermissionsByRole(string roleId)
    {
        IEnumerable<PermissionResponseDto> permissions = await _permissionService.GetPermissionsByRoleAsync(roleId);
        return Ok(permissions);
    }

    [HttpGet("module/{moduleId}")]
    [Authorize(Roles = "SuperAdministrador,Administrador")]
    [ProducesResponseType(typeof(IEnumerable<PermissionResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPermissionsByModule(int moduleId)
    {
        IEnumerable<PermissionResponseDto> permissions = await _permissionService.GetPermissionsByModuleAsync(moduleId);
        return Ok(permissions);
    }
}
