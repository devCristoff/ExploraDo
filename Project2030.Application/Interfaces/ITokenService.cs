using Project2030.Domain.Entities;

namespace Project2030.Application.Interfaces;

/// <summary>
/// Contrato para la generación y gestión de tokens de seguridad (JWT y recuperación de contraseña).
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Genera un token JWT firmado para el usuario autenticado.
    /// </summary>
    /// <param name="user">Usuario para el que se genera el token.</param>
    /// <param name="role">Rol principal del usuario, incluido en los claims.</param>
    string GenerateJwtToken(ApplicationUser user, string role);

    /// <summary>
    /// Genera y persiste un token de recuperación de contraseña de un solo uso.
    /// </summary>
    /// <param name="userId">Identificador del usuario.</param>
    Task<string> GeneratePasswordResetTokenAsync(string userId);

    /// <summary>
    /// Verifica que un token de recuperación sea válido, no esté usado y no haya expirado.
    /// </summary>
    /// <param name="userId">Identificador del usuario.</param>
    /// <param name="token">Token a validar.</param>
    Task<bool> ValidatePasswordResetTokenAsync(string userId, string token);

    /// <summary>
    /// Marca un token de recuperación como utilizado para prevenir su reutilización.
    /// </summary>
    /// <param name="userId">Identificador del usuario.</param>
    /// <param name="token">Token a invalidar.</param>
    Task InvalidatePasswordResetTokenAsync(string userId, string token);
}
