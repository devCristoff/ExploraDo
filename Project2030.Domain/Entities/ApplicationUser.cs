using Microsoft.AspNetCore.Identity;

namespace Project2030.Domain.Entities;

/// <summary>
/// Usuario de la aplicación que extiende IdentityUser con campos adicionales de perfil y estado.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>Nombre completo del usuario.</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>Número de teléfono del usuario (opcional).</summary>
    public string? Phone { get; set; }

    /// <summary>URL o ruta de la foto de perfil (opcional).</summary>
    public string? ProfilePhoto { get; set; }

    /// <summary>Indica si el usuario está bloqueado manualmente por un administrador.</summary>
    public bool IsBlocked { get; set; }

    /// <summary>Motivo del bloqueo cuando <see cref="IsBlocked"/> es verdadero (opcional).</summary>
    public string? BlockReason { get; set; }

    /// <summary>Fecha y hora UTC en que se creó el usuario.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Fecha y hora UTC de la última actualización del usuario (opcional).</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Registros de acceso asociados a este usuario.</summary>
    public ICollection<AccessLog> AccessLogs { get; set; } = new List<AccessLog>();

    /// <summary>Tokens de recuperación de contraseña asociados a este usuario.</summary>
    public ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
}
