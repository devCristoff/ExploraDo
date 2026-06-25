namespace Project2030.Application.DTOs.Auth;

/// <summary>
/// Datos requeridos para autenticar un usuario en el sistema.
/// </summary>
public class LoginRequestDto
{
    /// <summary>Correo electrónico del usuario.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Contraseña del usuario.</summary>
    public string Password { get; set; } = string.Empty;
}
