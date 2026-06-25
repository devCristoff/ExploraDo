using Project2030.Domain.Entities;

namespace Project2030.Application.Interfaces.Repositories;

public interface IPermissionRepository
{
    Task<bool> PermissionExistsAsync(string roleId, int moduleId);
    Task<Module?> GetModuleByIdAsync(int moduleId);
    Task<RolePermission?> GetByIdWithNavigationAsync(int permissionId);
    Task<IEnumerable<RolePermission>> GetByRoleIdAsync(string roleId);
    Task<IEnumerable<RolePermission>> GetByModuleIdAsync(int moduleId);
    Task AddAsync(RolePermission permission);
    Task SaveChangesAsync();
}
