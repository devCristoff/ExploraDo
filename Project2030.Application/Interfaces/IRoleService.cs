using Project2030.Application.DTOs.Roles;

namespace Project2030.Application.Interfaces;

public interface IRoleService
{
    Task<RoleResponseDto> CreateRoleAsync(CreateRoleRequestDto request);
    Task<RoleResponseDto> UpdateRoleAsync(string roleId, UpdateRoleRequestDto request);
    Task DeactivateRoleAsync(string roleId);
    Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync();
    Task<RoleResponseDto> GetRoleByIdAsync(string roleId);
}
