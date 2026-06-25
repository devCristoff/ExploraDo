using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project2030.Application.DTOs.Auth;
using Project2030.Application.Interfaces;

namespace Project2030.API.Controllers;

/// <summary>
/// Controlador para gestionar el bloqueo y desbloqueo de usuarios del sistema.
/// Solo accesible para roles Administrador y SuperAdministrador.
/// </summary>
[ApiController]
[Route("api/users")]
[Authorize(Roles = "Administrador,SuperAdministrador")]
public class UserBlockController : ControllerBase
{
    private readonly IUserManagementService _userManagementService;

    /// <summary>Inicializa una nueva instancia de <see cref="UserBlockController"/>.</summary>
    public UserBlockController(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    /// <summary>
    /// Bloquea un usuario impidiéndole iniciar sesión en el sistema.
    /// </summary>
    /// <param name="id">Identificador del usuario a bloquear.</param>
    /// <param name="request">Motivo del bloqueo.</param>
    [HttpPost("{id}/block")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> BlockUser(string id, [FromBody] BlockUserRequestDto request)
    {
        try
        {
            request.UserId = id;
            await _userManagementService.BlockUserAsync(request);
            return Ok(new { message = $"Usuario '{id}' bloqueado correctamente" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { statusCode = 404, message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { statusCode = 400, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { statusCode = 500, message = ex.Message });
        }
    }

    /// <summary>
    /// Desbloquea un usuario previamente bloqueado, restaurando su acceso al sistema.
    /// </summary>
    /// <param name="id">Identificador del usuario a desbloquear.</param>
    [HttpPost("{id}/unblock")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UnblockUser(string id)
    {
        try
        {
            await _userManagementService.UnblockUserAsync(id);
            return Ok(new { message = $"Usuario '{id}' desbloqueado correctamente" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { statusCode = 404, message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { statusCode = 400, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { statusCode = 500, message = ex.Message });
        }
    }
}
