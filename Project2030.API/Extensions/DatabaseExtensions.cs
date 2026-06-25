using Microsoft.EntityFrameworkCore;
using Project2030.Infrastructure.Data;

namespace Project2030.API.Extensions;

/// <summary>
/// Extension methods para registrar y configurar la base de datos.
/// </summary>
public static class DatabaseExtensions
{
    /// <summary>
    /// Registra AppDbContext con SQL Server usando la cadena de conexión DefaultConnection.
    /// </summary>
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }
}
