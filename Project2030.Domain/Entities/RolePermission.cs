namespace Project2030.Domain.Entities;

/// <summary>
/// Permisos granulares (CRUD) asignados a un rol sobre un módulo específico del sistema.
/// Cada combinación RoleId + ModuleId debe ser única.
/// </summary>
public class RolePermission
{
    /// <summary>Identificador único del permiso.</summary>
    public int Id { get; set; }

    /// <summary>FK hacia el rol al que se asignan estos permisos.</summary>
    public string RoleId { get; set; } = string.Empty;

    /// <summary>FK hacia el módulo sobre el que aplican los permisos.</summary>
    public int ModuleId { get; set; }

    /// <summary>Permiso para crear registros en el módulo.</summary>
    public bool CanCreate { get; set; }

    /// <summary>Permiso para leer registros en el módulo.</summary>
    public bool CanRead { get; set; }

    /// <summary>Permiso para actualizar registros en el módulo.</summary>
    public bool CanUpdate { get; set; }

    /// <summary>Permiso para eliminar registros en el módulo.</summary>
    public bool CanDelete { get; set; }

    /// <summary>Fecha y hora UTC en que se asignaron los permisos.</summary>
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Rol al que pertenece este permiso.</summary>
    public ApplicationRole Role { get; set; } = null!;

    /// <summary>Módulo al que aplica este permiso.</summary>
    public Module Module { get; set; } = null!;
}
