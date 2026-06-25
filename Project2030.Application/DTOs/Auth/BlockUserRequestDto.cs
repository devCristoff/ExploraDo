namespace Project2030.Application.DTOs.Auth;

/// <summary>
/// Datos requeridos para bloquear un usuario del sistema.
/// </summary>
public class BlockUserRequestDto
{
    /// <summary>Identificador del usuario a bloquear.</summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>Motivo del bloqueo.</summary>
    public string Reason { get; set; } = string.Empty;
}
