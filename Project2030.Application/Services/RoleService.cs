using Microsoft.AspNetCore.Identity;
using Project2030.Application.DTOs.Roles;
using Project2030.Application.Interfaces;
using Project2030.Application.Interfaces.Repositories;
using Project2030.Domain.Entities;

namespace Project2030.Application.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<RoleResponseDto> CreateRoleAsync(CreateRoleRequestDto request)
    {
        bool exists = await _roleRepository.ExistsAsync(request.Name);
        if (exists)
            throw new InvalidOperationException("Ya existe un rol con ese nombre");

        ApplicationRole role = new ApplicationRole
        {
            Name = request.Name,
            Description = request.Description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        IdentityResult result = await _roleRepository.CreateAsync(role);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        return MapToResponseDto(role);
    }

    public async Task<RoleResponseDto> UpdateRoleAsync(string roleId, UpdateRoleRequestDto request)
    {
        ApplicationRole? role = await _roleRepository.FindByIdAsync(roleId);
        if (role is null)
            throw new KeyNotFoundException("Rol no encontrado");

        role.Description = request.Description;
        role.IsActive = request.IsActive;

        await _roleRepository.UpdateAsync(role);

        return MapToResponseDto(role);
    }

    public async Task DeactivateRoleAsync(string roleId)
    {
        ApplicationRole? role = await _roleRepository.FindByIdAsync(roleId);
        if (role is null)
            throw new KeyNotFoundException("Rol no encontrado");

        IList<ApplicationUser> usersInRole = await _roleRepository.GetUsersInRoleAsync(role.Name!);
        bool hasActiveUsers = usersInRole.Any(u => !u.IsBlocked);

        if (hasActiveUsers)
            throw new InvalidOperationException("No se puede desactivar un rol con usuarios activos asignados");

        role.IsActive = false;
        await _roleRepository.UpdateAsync(role);
    }

    public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync()
    {
        IEnumerable<ApplicationRole> roles = await _roleRepository.GetAllAsync();
        return roles.Select(MapToResponseDto);
    }

    public async Task<RoleResponseDto> GetRoleByIdAsync(string roleId)
    {
        ApplicationRole? role = await _roleRepository.FindByIdAsync(roleId);
        if (role is null)
            throw new KeyNotFoundException("Rol no encontrado");

        return MapToResponseDto(role);
    }

    private static RoleResponseDto MapToResponseDto(ApplicationRole role) =>
        new RoleResponseDto
        {
            Id = role.Id,
            Name = role.Name ?? string.Empty,
            Description = role.Description,
            IsActive = role.IsActive,
            CreatedAt = role.CreatedAt
        };
}
