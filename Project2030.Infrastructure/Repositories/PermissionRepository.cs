using Microsoft.EntityFrameworkCore;
using Project2030.Application.Interfaces.Repositories;
using Project2030.Domain.Entities;
using Project2030.Infrastructure.Data;

namespace Project2030.Infrastructure.Repositories;

public class PermissionRepository : IPermissionRepository
{
    private readonly AppDbContext _context;

    public PermissionRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<bool> PermissionExistsAsync(string roleId, int moduleId) =>
        _context.RolePermissions.AnyAsync(rp => rp.RoleId == roleId && rp.ModuleId == moduleId);

    public async Task<Module?> GetModuleByIdAsync(int moduleId) =>
        await _context.Modules.FindAsync(moduleId);

    public Task<RolePermission?> GetByIdWithNavigationAsync(int permissionId) =>
        _context.RolePermissions
            .Include(rp => rp.Role)
            .Include(rp => rp.Module)
            .FirstOrDefaultAsync(rp => rp.Id == permissionId);

    public async Task<IEnumerable<RolePermission>> GetByRoleIdAsync(string roleId) =>
        await _context.RolePermissions
            .Include(rp => rp.Role)
            .Include(rp => rp.Module)
            .Where(rp => rp.RoleId == roleId)
            .OrderBy(rp => rp.Module.Name)
            .ToListAsync();

    public async Task<IEnumerable<RolePermission>> GetByModuleIdAsync(int moduleId) =>
        await _context.RolePermissions
            .Include(rp => rp.Role)
            .Include(rp => rp.Module)
            .Where(rp => rp.ModuleId == moduleId)
            .OrderBy(rp => rp.Role.Name)
            .ToListAsync();

    public Task AddAsync(RolePermission permission)
    {
        _context.RolePermissions.Add(permission);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() =>
        _context.SaveChangesAsync();
}
