using Microsoft.AspNetCore.Identity;
using Project2030.Application.DTOs.Profile;
using Project2030.Application.DTOs.Users;
using Project2030.Application.Interfaces;
using Project2030.Application.Interfaces.Repositories;
using Project2030.Domain.Entities;

namespace Project2030.Application.Services;

public class ProfileService : IProfileService
{
    private readonly IUserRepository _userRepository;

    public ProfileService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponseDto> GetProfileAsync(string userId)
    {
        ApplicationUser? user = await _userRepository.FindByIdAsync(userId);
        if (user is null)
            throw new KeyNotFoundException("Usuario no encontrado");

        return await MapToResponseDto(user);
    }

    public async Task<UserResponseDto> UpdateProfileAsync(string userId, UpdateProfileRequestDto request)
    {
        ApplicationUser? user = await _userRepository.FindByIdAsync(userId);
        if (user is null)
            throw new KeyNotFoundException("Usuario no encontrado");

        user.FullName = request.FullName;
        user.Phone = request.Phone;
        user.ProfilePhoto = request.ProfilePhoto;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

        return await MapToResponseDto(user);
    }

    public async Task ChangePasswordAsync(string userId, ChangePasswordRequestDto request)
    {
        ApplicationUser? user = await _userRepository.FindByIdAsync(userId);
        if (user is null)
            throw new KeyNotFoundException("Usuario no encontrado");

        if (request.NewPassword != request.ConfirmNewPassword)
            throw new ArgumentException("La nueva contraseña y la confirmación no coinciden");

        IdentityResult result = await _userRepository.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);
    }

    private async Task<UserResponseDto> MapToResponseDto(ApplicationUser user)
    {
        IList<string> roles = await _userRepository.GetRolesAsync(user);
        return new UserResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            Phone = user.Phone,
            ProfilePhoto = user.ProfilePhoto,
            IsBlocked = user.IsBlocked,
            BlockReason = user.BlockReason,
            Role = roles.FirstOrDefault() ?? string.Empty,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}
