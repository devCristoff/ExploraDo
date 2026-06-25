using Project2030.Application.DTOs.Auth;

namespace Project2030.Application.Interfaces;

/// <summary>
/// Contrato para las operaciones de administración del estado de los usuarios.
/// </summary>
public interface IUserManagementService
{
    /// <summary>
    /// Bloquea un usuario impidiéndole iniciar sesión en el sistema.
    /// </summary>
    /// <param name="request">Datos del bloqueo incluyendo el Id y el motivo.</param>
    Task BlockUserAsync(BlockUserRequestDto request);

    /// <summary>
    /// Desbloquea un usuario previamente bloqueado, restaurando su acceso al sistema.
    /// </summary>
    /// <param name="userId">Identificador del usuario a desbloquear.</param>
    Task UnblockUserAsync(string userId);
}
