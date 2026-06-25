namespace Project2030.Domain.Entities;

/// <summary>
/// Módulo del sistema al que se pueden asignar permisos granulares por rol.
/// </summary>
public class Module
{
    /// <summary>Identificador único del módulo.</summary>
    public int Id { get; set; }

    /// <summary>Nombre del módulo (ej: "Usuarios", "Roles").</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Descripción del propósito del módulo (opcional).</summary>
    public string? Description { get; set; }

    /// <summary>Indica si el módulo está activo en el sistema.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Fecha y hora UTC en que se creó el módulo.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Permisos de roles asociados a este módulo.</summary>
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
