using System.Net;
using System.Text.Json;

namespace Project2030.API.Middleware;

/// <summary>
/// Middleware global de manejo de excepciones no capturadas.
/// Intercepta cualquier excepción y retorna una respuesta JSON estructurada con el código HTTP apropiado.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    /// <summary>Inicializa una nueva instancia de <see cref="ExceptionMiddleware"/>.</summary>
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invoca el siguiente middleware en el pipeline y captura cualquier excepción no manejada.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción no manejada: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        (int statusCode, string message) = exception switch
        {
            UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, exception.Message),
            KeyNotFoundException => ((int)HttpStatusCode.NotFound, exception.Message),
            ArgumentException => ((int)HttpStatusCode.BadRequest, exception.Message),
            InvalidOperationException => ((int)HttpStatusCode.BadRequest, exception.Message),
            _ => ((int)HttpStatusCode.InternalServerError, "Ocurrió un error interno en el servidor")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        string json = JsonSerializer.Serialize(new
        {
            statusCode,
            message
        });

        await context.Response.WriteAsync(json);
    }
}
