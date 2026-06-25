using Project2030.Domain.Entities;

namespace Project2030.Application.Interfaces.Repositories;

public interface IPasswordResetTokenRepository
{
    Task AddAsync(PasswordResetToken token);
    Task<PasswordResetToken?> FindAsync(string userId, string token);
    Task SaveChangesAsync();
}
