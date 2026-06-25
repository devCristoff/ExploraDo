using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project2030.Application.Interfaces.Repositories;
using Project2030.Domain.Entities;

namespace Project2030.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoleRepository(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public Task<ApplicationRole?> FindByIdAsync(string roleId) =>
        _roleManager.FindByIdAsync(roleId);

    public Task<ApplicationRole?> FindByNameAsync(string roleName) =>
        _roleManager.FindByNameAsync(roleName);

    public async Task<IEnumerable<ApplicationRole>> GetAllAsync() =>
        await _roleManager.Roles.ToListAsync();

    public Task<bool> ExistsAsync(string roleName) =>
        _roleManager.RoleExistsAsync(roleName);

    public Task<IdentityResult> CreateAsync(ApplicationRole role) =>
        _roleManager.CreateAsync(role);

    public Task<IdentityResult> UpdateAsync(ApplicationRole role) =>
        _roleManager.UpdateAsync(role);

    public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName) =>
        _userManager.GetUsersInRoleAsync(roleName);
}
