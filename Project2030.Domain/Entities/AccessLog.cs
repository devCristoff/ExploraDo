namespace Project2030.Domain.Entities;

/// <summary>
/// Registro de auditoría que captura cada intento de acceso (login/logout) al sistema.
/// </summary>
public class AccessLog
{
    /// <summary>Identificador único del registro de acceso.</summary>
    public int Id { get; set; }

    /// <summary>FK hacia el usuario que realizó el acceso.</summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>Fecha y hora UTC en que ocurrió el acceso.</summary>
    public DateTime AccessTime { get; set; } = DateTime.UtcNow;

    /// <summary>Dirección IP desde la que se realizó el acceso (opcional).</summary>
    public string? IpAddress { get; set; }

    /// <summary>Localización geográfica derivada de la IP (opcional).</summary>
    public string? Location { get; set; }

    /// <summary>Acción realizada: "LOGIN" o "LOGOUT".</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>Indica si el acceso fue exitoso.</summary>
    public bool Success { get; set; }

    /// <summary>Motivo del fallo cuando <see cref="Success"/> es falso (opcional).</summary>
    public string? FailureReason { get; set; }

    /// <summary>Usuario al que pertenece este registro de acceso.</summary>
    public ApplicationUser User { get; set; } = null!;
}
