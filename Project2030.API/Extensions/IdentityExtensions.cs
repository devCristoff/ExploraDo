using Microsoft.AspNetCore.Identity;
using Project2030.Domain.Entities;
using Project2030.Infrastructure.Data;

namespace Project2030.API.Extensions;

/// <summary>
/// Extension methods para registrar y configurar ASP.NET Core Identity.
/// </summary>
public static class IdentityExtensions
{
    /// <summary>
    /// Registra Identity con <see cref="ApplicationUser"/> y <see cref="ApplicationRole"/>,
    /// configurando las políticas de contraseña requeridas.
    /// </summary>
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;

            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}
