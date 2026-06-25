using Microsoft.OpenApi.Models;

namespace Project2030.API.Extensions;

/// <summary>
/// Extension methods para registrar y configurar Swagger/OpenAPI.
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Registra los servicios de Swagger con soporte para autenticación JWT Bearer.
    /// </summary>
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Project2030 API",
                Version = "v1",
                Description = "Sistema de Administración de Usuarios y Accesos - API REST"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Ingresa el token JWT"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    /// <summary>
    /// Habilita el middleware de Swagger UI en el pipeline HTTP.
    /// </summary>
    public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Project2030 API v1");
            options.RoutePrefix = "swagger";
        });

        return app;
    }
}
