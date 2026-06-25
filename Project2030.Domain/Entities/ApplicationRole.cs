using Microsoft.AspNetCore.Identity;

namespace Project2030.Domain.Entities;

/// <summary>
/// Rol de la aplicación que extiende IdentityRole con descripción y estado de activación.
/// </summary>
public class ApplicationRole : IdentityRole
{
    /// <summary>Descripción del rol y sus responsabilidades (opcional).</summary>
    public string? Description { get; set; }

    /// <summary>Indica si el rol está activo y puede ser asignado.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Fecha y hora UTC en que se creó el rol.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Permisos por módulo asignados a este rol.</summary>
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
