using Project2030.Application.DTOs.Auth;

namespace Project2030.Application.Interfaces;

/// <summary>
/// Contrato para las operaciones de autenticación y gestión de sesión.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Autentica un usuario con email y contraseña, retornando un JWT si las credenciales son válidas.
    /// </summary>
    /// <param name="request">Credenciales del usuario.</param>
    /// <param name="ipAddress">Dirección IP desde la que se origina la solicitud.</param>
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request, string ipAddress);

    /// <summary>
    /// Registra el cierre de sesión del usuario en el log de auditoría.
    /// </summary>
    /// <param name="userId">Identificador del usuario que cierra sesión.</param>
    /// <param name="ipAddress">Dirección IP desde la que se origina la solicitud.</param>
    Task LogoutAsync(string userId, string ipAddress);

    /// <summary>
    /// Inicia el flujo de recuperación de contraseña generando un token y notificando al usuario.
    /// </summary>
    /// <param name="request">Email del usuario que solicita la recuperación.</param>
    Task ForgotPasswordAsync(ForgotPasswordRequestDto request);

    /// <summary>
    /// Restablece la contraseña del usuario usando el token de recuperación.
    /// </summary>
    /// <param name="request">Token, email y nueva contraseña.</param>
    Task ResetPasswordAsync(ResetPasswordRequestDto request);
}
