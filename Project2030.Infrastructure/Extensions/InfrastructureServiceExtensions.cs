using Microsoft.Extensions.DependencyInjection;
using Project2030.Application.Interfaces;
using Project2030.Application.Interfaces.Repositories;
using Project2030.Application.Services;
using Project2030.Infrastructure.Repositories;
using Project2030.Infrastructure.Services;

namespace Project2030.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IAccessLogRepository, AccessLogRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();

        // Infrastructure-only services
        services.AddScoped<IGeolocationService, GeolocationService>();
        services.AddHttpClient();

        // Application services
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAccessLogService, AccessLogService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserManagementService, UserManagementService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IProfileService, ProfileService>();

        return services;
    }
}
