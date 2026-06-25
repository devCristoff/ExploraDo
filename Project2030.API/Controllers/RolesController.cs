using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project2030.Application.DTOs.Roles;
using Project2030.Application.Interfaces;

namespace Project2030.API.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdministrador")]
    [ProducesResponseType(typeof(RoleResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequestDto request)
    {
        RoleResponseDto role = await _roleService.CreateRoleAsync(request);
        return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdministrador")]
    [ProducesResponseType(typeof(RoleResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRole(string id, [FromBody] UpdateRoleRequestDto request)
    {
        RoleResponseDto role = await _roleService.UpdateRoleAsync(id, request);
        return Ok(role);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdministrador")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeactivateRole(string id)
    {
        await _roleService.DeactivateRoleAsync(id);
        return NoContent();
    }

    [HttpGet]
    [Authorize(Roles = "SuperAdministrador,Administrador")]
    [ProducesResponseType(typeof(IEnumerable<RoleResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllRoles()
    {
        IEnumerable<RoleResponseDto> roles = await _roleService.GetAllRolesAsync();
        return Ok(roles);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "SuperAdministrador,Administrador")]
    [ProducesResponseType(typeof(RoleResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoleById(string id)
    {
        RoleResponseDto role = await _roleService.GetRoleByIdAsync(id);
        return Ok(role);
    }
}
