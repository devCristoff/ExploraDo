using Microsoft.AspNetCore.Identity;
using Project2030.Domain.Entities;

namespace Project2030.Application.Interfaces.Repositories;

public interface IRoleRepository
{
    Task<ApplicationRole?> FindByIdAsync(string roleId);
    Task<ApplicationRole?> FindByNameAsync(string roleName);
    Task<IEnumerable<ApplicationRole>> GetAllAsync();
    Task<bool> ExistsAsync(string roleName);
    Task<IdentityResult> CreateAsync(ApplicationRole role);
    Task<IdentityResult> UpdateAsync(ApplicationRole role);
    Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName);
}
