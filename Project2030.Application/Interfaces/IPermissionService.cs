using Project2030.Application.DTOs.Permissions;

namespace Project2030.Application.Interfaces;

public interface IPermissionService
{
    Task<PermissionResponseDto> AssignPermissionAsync(AssignPermissionRequestDto request);
    Task<PermissionResponseDto> UpdatePermissionAsync(int permissionId, AssignPermissionRequestDto request);
    Task<IEnumerable<PermissionResponseDto>> GetPermissionsByRoleAsync(string roleId);
    Task<IEnumerable<PermissionResponseDto>> GetPermissionsByModuleAsync(int moduleId);
}
