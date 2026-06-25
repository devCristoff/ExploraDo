using Microsoft.EntityFrameworkCore;
using Project2030.Application.Interfaces.Repositories;
using Project2030.Domain.Entities;
using Project2030.Infrastructure.Data;

namespace Project2030.Infrastructure.Repositories;

public class PasswordResetTokenRepository : IPasswordResetTokenRepository
{
    private readonly AppDbContext _context;

    public PasswordResetTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(PasswordResetToken token)
    {
        _context.PasswordResetTokens.Add(token);
        return Task.CompletedTask;
    }

    public Task<PasswordResetToken?> FindAsync(string userId, string token) =>
        _context.PasswordResetTokens
            .FirstOrDefaultAsync(t => t.UserId == userId && t.Token == token);

    public Task SaveChangesAsync() =>
        _context.SaveChangesAsync();
}
