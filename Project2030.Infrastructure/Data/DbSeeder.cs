using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project2030.Domain.Entities;

namespace Project2030.Infrastructure.Data;

/// <summary>
/// Encargado de poblar la base de datos con datos iniciales requeridos para el funcionamiento del sistema.
/// </summary>
public static class DbSeeder
{
    private static readonly string[] RoleNames =
    [
        "SuperAdministrador",
        "Administrador",
        "Cliente",
        "Supervisor"
    ];

    private static readonly (string Name, string Description)[] ModuleDefinitions =
    [
        ("Usuarios",   "Gestión de usuarios del sistema"),
        ("Roles",      "Gestión de roles y asignaciones"),
        ("Permisos",   "Configuración de permisos granulares"),
        ("Auditoría",  "Consulta de registros de acceso y auditoría"),
        ("Perfil",     "Gestión del perfil del usuario autenticado")
    ];

    /// <summary>
    /// Ejecuta la siembra de roles, módulos, usuario SuperAdmin y sus permisos completos.
    /// Debe llamarse al inicio de la aplicación desde Program.cs.
    /// </summary>
    /// <param name="roleManager">Gestor de roles de Identity.</param>
    /// <param name="userManager">Gestor de usuarios de Identity.</param>
    /// <param name="dbContext">Contexto de base de datos.</param>
    public static async Task SeedAsync(
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager,
        AppDbContext dbContext)
    {
        await SeedRolesAsync(roleManager);
        await SeedModulesAsync(dbContext);
        await SeedSuperAdminUserAsync(userManager, roleManager, dbContext);
    }

    private static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
    {
        foreach (string roleName in RoleNames)
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                continue;
            }

            ApplicationRole role = new()
            {
                Name = roleName,
                Description = $"Rol de {roleName}",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await roleManager.CreateAsync(role);
        }
    }

    private static async Task SeedModulesAsync(AppDbContext dbContext)
    {
        foreach ((string name, string description) in ModuleDefinitions)
        {
            bool exists = await dbContext.Modules.AnyAsync(m => m.Name == name);
            if (exists)
            {
                continue;
            }

            dbContext.Modules.Add(new Module
            {
                Name = name,
                Description = description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
        }

        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedSuperAdminUserAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        AppDbContext dbContext)
    {
        const string superAdminEmail = "superadmin@system.local";
        const string superAdminPassword = "Admin@12345";
        const string superAdminRoleName = "SuperAdministrador";

        ApplicationUser? existingUser = await userManager.FindByEmailAsync(superAdminEmail);
        if (existingUser is null)
        {
            ApplicationUser superAdmin = new()
            {
                FullName = "Super Administrador",
                Email = superAdminEmail,
                UserName = superAdminEmail,
                EmailConfirmed = true,
                IsBlocked = false,
                CreatedAt = DateTime.UtcNow
            };

            IdentityResult result = await userManager.CreateAsync(superAdmin, superAdminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(superAdmin, superAdminRoleName);
            }
        }

        await SeedSuperAdminPermissionsAsync(roleManager, dbContext, superAdminRoleName);
    }

    private static async Task SeedSuperAdminPermissionsAsync(
        RoleManager<ApplicationRole> roleManager,
        AppDbContext dbContext,
        string superAdminRoleName)
    {
        ApplicationRole? superAdminRole = await roleManager.FindByNameAsync(superAdminRoleName);
        if (superAdminRole is null)
        {
            return;
        }

        List<Module> modules = await dbContext.Modules.ToListAsync();

        foreach (Module module in modules)
        {
            bool permissionExists = await dbContext.RolePermissions
                .AnyAsync(rp => rp.RoleId == superAdminRole.Id && rp.ModuleId == module.Id);

            if (permissionExists)
            {
                continue;
            }

            dbContext.RolePermissions.Add(new RolePermission
            {
                RoleId = superAdminRole.Id,
                ModuleId = module.Id,
                CanCreate = true,
                CanRead = true,
                CanUpdate = true,
                CanDelete = true,
                AssignedAt = DateTime.UtcNow
            });
        }

        await dbContext.SaveChangesAsync();
    }
}
