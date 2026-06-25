using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project2030.Domain.Entities;

namespace Project2030.Infrastructure.Data;

/// <summary>
/// Contexto principal de la base de datos. Extiende IdentityDbContext para integrar
/// ASP.NET Core Identity con las entidades custom del sistema.
/// </summary>
public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    /// <summary>Inicializa una nueva instancia de <see cref="AppDbContext"/>.</summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>Módulos del sistema.</summary>
    public DbSet<Module> Modules { get; set; }

    /// <summary>Permisos granulares por rol y módulo.</summary>
    public DbSet<RolePermission> RolePermissions { get; set; }

    /// <summary>Registros de auditoría de accesos.</summary>
    public DbSet<AccessLog> AccessLogs { get; set; }

    /// <summary>Tokens de recuperación de contraseña.</summary>
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    /// <summary>
    /// Configura el modelo de datos: relaciones, restricciones e índices.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>().ToTable("Users");
        builder.Entity<ApplicationRole>().ToTable("Roles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

        // RolePermission → ApplicationRole
        builder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        // RolePermission → Module
        builder.Entity<RolePermission>()
            .HasOne(rp => rp.Module)
            .WithMany(m => m.RolePermissions)
            .HasForeignKey(rp => rp.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índice único: un rol no puede tener dos registros para el mismo módulo
        builder.Entity<RolePermission>()
            .HasIndex(rp => new { rp.RoleId, rp.ModuleId })
            .IsUnique();

        // AccessLog → ApplicationUser (Restrict para preservar historial al eliminar usuario)
        builder.Entity<AccessLog>()
            .HasOne(al => al.User)
            .WithMany(u => u.AccessLogs)
            .HasForeignKey(al => al.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // PasswordResetToken → ApplicationUser (Cascade para limpiar tokens al eliminar usuario)
        builder.Entity<PasswordResetToken>()
            .HasOne(prt => prt.User)
            .WithMany(u => u.PasswordResetTokens)
            .HasForeignKey(prt => prt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
