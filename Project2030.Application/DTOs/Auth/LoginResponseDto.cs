namespace Project2030.Application.DTOs.Auth;

/// <summary>
/// Resultado exitoso de autenticación, incluyendo el JWT y datos del usuario.
/// </summary>
public class LoginResponseDto
{
    /// <summary>Token JWT firmado para autenticar solicitudes posteriores.</summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>Nombre completo del usuario autenticado.</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>Correo electrónico del usuario autenticado.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Rol principal del usuario autenticado.</summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>Fecha y hora UTC en que expira el token.</summary>
    public DateTime ExpiresAt { get; set; }
}
