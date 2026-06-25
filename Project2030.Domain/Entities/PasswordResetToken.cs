namespace Project2030.Domain.Entities;

/// <summary>
/// Token de un solo uso para el proceso de recuperación de contraseña.
/// </summary>
public class PasswordResetToken
{
    /// <summary>Identificador único del token.</summary>
    public int Id { get; set; }

    /// <summary>FK hacia el usuario que solicitó el restablecimiento.</summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>Token único generado para el restablecimiento.</summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>Fecha y hora UTC en que expira el token.</summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>Indica si el token ya fue utilizado.</summary>
    public bool IsUsed { get; set; }

    /// <summary>Fecha y hora UTC en que se creó el token.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Usuario al que pertenece este token.</summary>
    public ApplicationUser User { get; set; } = null!;
}
