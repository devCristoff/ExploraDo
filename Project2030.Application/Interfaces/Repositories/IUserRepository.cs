using Microsoft.AspNetCore.Identity;
using Project2030.Domain.Entities;

namespace Project2030.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<ApplicationUser?> FindByIdAsync(string userId);
    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task<IEnumerable<ApplicationUser>> GetAllAsync();
    Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
    Task<IdentityResult> UpdateAsync(ApplicationUser user);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
    Task<IdentityResult> RemovePasswordAsync(ApplicationUser user);
    Task<IdentityResult> AddPasswordAsync(ApplicationUser user, string newPassword);
    Task<IList<string>> GetRolesAsync(ApplicationUser user);
    Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName);
}
