using Microsoft.AspNetCore.Identity;
using Project2030.Application.DTOs.Users;
using Project2030.Application.Interfaces;
using Project2030.Application.Interfaces.Repositories;
using Project2030.Domain.Entities;

namespace Project2030.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto request)
    {
        ApplicationUser? existing = await _userRepository.FindByEmailAsync(request.Email);
        if (existing is not null)
            throw new InvalidOperationException("El correo ya está registrado");

        ApplicationRole? role = await _roleRepository.FindByNameAsync(request.RoleName);
        if (role is null)
            throw new KeyNotFoundException("Rol no encontrado");

        if (!role.IsActive)
            throw new InvalidOperationException("El rol está inactivo");

        ApplicationUser user = new ApplicationUser
        {
            FullName = request.FullName,
            Email = request.Email,
            UserName = request.Email,
            Phone = request.Phone,
            IsBlocked = false,
            CreatedAt = DateTime.UtcNow
        };

        IdentityResult result = await _userRepository.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userRepository.AddToRoleAsync(user, request.RoleName);

        return await MapToResponseDto(user);
    }

    public async Task<UserResponseDto> GetUserByIdAsync(string userId)
    {
        ApplicationUser? user = await _userRepository.FindByIdAsync(userId);
        if (user is null)
            throw new KeyNotFoundException("Usuario no encontrado");

        return await MapToResponseDto(user);
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
    {
        IEnumerable<ApplicationUser> users = await _userRepository.GetAllAsync();
        List<UserResponseDto> result = new List<UserResponseDto>();

        foreach (ApplicationUser user in users)
            result.Add(await MapToResponseDto(user));

        return result;
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
