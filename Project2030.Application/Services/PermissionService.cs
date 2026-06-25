using Project2030.Application.DTOs.Permissions;
using Project2030.Application.Interfaces;
using Project2030.Application.Interfaces.Repositories;
using Project2030.Domain.Entities;

namespace Project2030.Application.Services;

public class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IRoleRepository _roleRepository;

    public PermissionService(IPermissionRepository permissionRepository, IRoleRepository roleRepository)
    {
        _permissionRepository = permissionRepository;
        _roleRepository = roleRepository;
    }

    public async Task<PermissionResponseDto> AssignPermissionAsync(AssignPermissionRequestDto request)
    {
        ApplicationRole? role = await _roleRepository.FindByIdAsync(request.RoleId);
        if (role is null)
            throw new KeyNotFoundException("Rol no encontrado");

        Module? module = await _permissionRepository.GetModuleByIdAsync(request.ModuleId);
        if (module is null)
            throw new KeyNotFoundException("Módulo no encontrado");

        bool alreadyExists = await _permissionRepository.PermissionExistsAsync(request.RoleId, request.ModuleId);
        if (alreadyExists)
            throw new InvalidOperationException("Ya existe un permiso para este rol y módulo. Use el endpoint de actualización");

        RolePermission permission = new RolePermission
        {
            RoleId = request.RoleId,
            ModuleId = request.ModuleId,
            CanCreate = request.CanCreate,
            CanRead = request.CanRead,
            CanUpdate = request.CanUpdate,
            CanDelete = request.CanDelete,
            AssignedAt = DateTime.UtcNow
        };

        await _permissionRepository.AddAsync(permission);
        await _permissionRepository.SaveChangesAsync();

        return new PermissionResponseDto
        {
            Id = permission.Id,
            RoleId = role.Id,
            RoleName = role.Name ?? string.Empty,
            ModuleId = module.Id,
            ModuleName = module.Name,
            CanCreate = permission.CanCreate,
            CanRead = permission.CanRead,
            CanUpdate = permission.CanUpdate,
            CanDelete = permission.CanDelete,
            AssignedAt = permission.AssignedAt
        };
    }

    public async Task<PermissionResponseDto> UpdatePermissionAsync(int permissionId, AssignPermissionRequestDto request)
    {
        RolePermission? permission = await _permissionRepository.GetByIdWithNavigationAsync(permissionId);
        if (permission is null)
            throw new KeyNotFoundException("Permiso no encontrado");

        permission.CanCreate = request.CanCreate;
        permission.CanRead = request.CanRead;
        permission.CanUpdate = request.CanUpdate;
        permission.CanDelete = request.CanDelete;

        await _permissionRepository.SaveChangesAsync();

        return MapToResponseDto(permission);
    }

    public async Task<IEnumerable<PermissionResponseDto>> GetPermissionsByRoleAsync(string roleId)
    {
        IEnumerable<RolePermission> permissions = await _permissionRepository.GetByRoleIdAsync(roleId);
        return permissions.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<PermissionResponseDto>> GetPermissionsByModuleAsync(int moduleId)
    {
        IEnumerable<RolePermission> permissions = await _permissionRepository.GetByModuleIdAsync(moduleId);
        return permissions.Select(MapToResponseDto);
    }

    private static PermissionResponseDto MapToResponseDto(RolePermission rp) =>
        new PermissionResponseDto
        {
            Id = rp.Id,
            RoleId = rp.RoleId,
            RoleName = rp.Role.Name ?? string.Empty,
            ModuleId = rp.ModuleId,
            ModuleName = rp.Module.Name,
            CanCreate = rp.CanCreate,
            CanRead = rp.CanRead,
            CanUpdate = rp.CanUpdate,
            CanDelete = rp.CanDelete,
            AssignedAt = rp.AssignedAt
        };
}
