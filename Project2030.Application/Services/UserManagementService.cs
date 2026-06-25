using Microsoft.AspNetCore.Identity;
using Project2030.Application.DTOs.Auth;
using Project2030.Application.Interfaces;
using Project2030.Application.Interfaces.Repositories;
using Project2030.Domain.Entities;

namespace Project2030.Application.Services;

public class UserManagementService : IUserManagementService
{
    private readonly IUserRepository _userRepository;

    public UserManagementService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task BlockUserAsync(BlockUserRequestDto request)
    {
        ApplicationUser? user = await _userRepository.FindByIdAsync(request.UserId);
        if (user is null)
            throw new KeyNotFoundException($"Usuario con Id '{request.UserId}' no encontrado");

        user.IsBlocked = true;
        user.BlockReason = request.Reason;
        user.UpdatedAt = DateTime.UtcNow;

        IdentityResult result = await _userRepository.UpdateAsync(user);
        if (!result.Succeeded)
        {
            string errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"No se pudo bloquear el usuario: {errors}");
        }
    }

    public async Task UnblockUserAsync(string userId)
    {
        ApplicationUser? user = await _userRepository.FindByIdAsync(userId);
        if (user is null)
            throw new KeyNotFoundException($"Usuario con Id '{userId}' no encontrado");

        user.IsBlocked = false;
        user.BlockReason = null;
        user.UpdatedAt = DateTime.UtcNow;

        IdentityResult result = await _userRepository.UpdateAsync(user);
        if (!result.Succeeded)
        {
            string errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"No se pudo desbloquear el usuario: {errors}");
        }
    }
}
