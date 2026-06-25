namespace Project2030.Application.DTOs.Auth;

/// <summary>
/// Datos requeridos para solicitar el restablecimiento de contraseña.
/// </summary>
public class ForgotPasswordRequestDto
{
    /// <summary>Correo electrónico de la cuenta cuya contraseña se desea recuperar.</summary>
    public string Email { get; set; } = string.Empty;
}
