using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project2030.Application.DTOs.Auth;
using Project2030.Application.Interfaces;

namespace Project2030.API.Controllers;

/// <summary>
/// Controlador responsable de autenticación, recuperación y restablecimiento de contraseña.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>Inicializa una nueva instancia de <see cref="AuthController"/>.</summary>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Autentica un usuario con email y contraseña, retornando un JWT válido.
    /// </summary>
    /// <param name="request">Credenciales del usuario.</param>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            LoginResponseDto response = await _authService.LoginAsync(request, ipAddress);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { statusCode = 401, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { statusCode = 500, message = ex.Message });
        }
    }

    /// <summary>
    /// Registra el cierre de sesión del usuario autenticado en la auditoría.
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Logout()
    {
        try
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            await _authService.LogoutAsync(userId, ipAddress);
            return Ok(new { message = "Sesión cerrada correctamente" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { statusCode = 500, message = ex.Message });
        }
    }

    /// <summary>
    /// Inicia el flujo de recuperación de contraseña para el email indicado.
    /// </summary>
    /// <param name="request">Email del usuario que solicita la recuperación.</param>
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
    {
        try
        {
            await _authService.ForgotPasswordAsync(request);
            return Ok(new { message = "Si el email existe en el sistema, recibirás instrucciones para restablecer tu contraseña" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { statusCode = 500, message = ex.Message });
        }
    }

    /// <summary>
    /// Restablece la contraseña del usuario usando un token de recuperación válido.
    /// </summary>
    /// <param name="request">Token, email y nueva contraseña.</param>
    [HttpPost("reset-password")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
    {
        try
        {
            await _authService.ResetPasswordAsync(request);
            return Ok(new { message = "Contraseña restablecida correctamente" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { statusCode = 400, message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { statusCode = 400, message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { statusCode = 404, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { statusCode = 500, message = ex.Message });
        }
    }
}
