namespace Project2030.Application.DTOs.Auth;

/// <summary>
/// Datos requeridos para restablecer la contraseña usando un token de recuperación.
/// </summary>
public class ResetPasswordRequestDto
{
    /// <summary>Token de recuperación recibido por el usuario.</summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>Correo electrónico del usuario que solicita el restablecimiento.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Nueva contraseña deseada.</summary>
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>Confirmación de la nueva contraseña (debe coincidir con <see cref="NewPassword"/>).</summary>
    public string ConfirmPassword { get; set; } = string.Empty;
}
