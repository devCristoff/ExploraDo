using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project2030.Application.Interfaces.Repositories;
using Project2030.Domain.Entities;

namespace Project2030.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public Task<ApplicationUser?> FindByIdAsync(string userId) =>
        _userManager.FindByIdAsync(userId);

    public Task<ApplicationUser?> FindByEmailAsync(string email) =>
        _userManager.FindByEmailAsync(email);

    public async Task<IEnumerable<ApplicationUser>> GetAllAsync() =>
        await _userManager.Users.ToListAsync();

    public Task<IdentityResult> CreateAsync(ApplicationUser user, string password) =>
        _userManager.CreateAsync(user, password);

    public Task<IdentityResult> UpdateAsync(ApplicationUser user) =>
        _userManager.UpdateAsync(user);

    public Task<bool> CheckPasswordAsync(ApplicationUser user, string password) =>
        _userManager.CheckPasswordAsync(user, password);

    public Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword) =>
        _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

    public Task<IdentityResult> RemovePasswordAsync(ApplicationUser user) =>
        _userManager.RemovePasswordAsync(user);

    public Task<IdentityResult> AddPasswordAsync(ApplicationUser user, string newPassword) =>
        _userManager.AddPasswordAsync(user, newPassword);

    public Task<IList<string>> GetRolesAsync(ApplicationUser user) =>
        _userManager.GetRolesAsync(user);

    public Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName) =>
        _userManager.AddToRoleAsync(user, roleName);
}
