using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project2030.Application.Interfaces;
using Project2030.Application.Interfaces.Repositories;
using Project2030.Domain.Entities;

namespace Project2030.Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IPasswordResetTokenRepository _tokenRepository;

    public TokenService(IConfiguration configuration, IPasswordResetTokenRepository tokenRepository)
    {
        _configuration = configuration;
        _tokenRepository = tokenRepository;
    }

    public string GenerateJwtToken(ApplicationUser user, string role)
    {
        string key = _configuration["Jwt:Key"]!;
        string issuer = _configuration["Jwt:Issuer"]!;
        string audience = _configuration["Jwt:Audience"]!;
        int expiresInMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"]!);

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GeneratePasswordResetTokenAsync(string userId)
    {
        int expiresInMinutes = int.Parse(_configuration["Jwt:PasswordResetExpiresInMinutes"]!);
        string token = Guid.NewGuid().ToString("N");

        PasswordResetToken resetToken = new PasswordResetToken
        {
            UserId = userId,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes),
            IsUsed = false,
            CreatedAt = DateTime.UtcNow
        };

        await _tokenRepository.AddAsync(resetToken);
        await _tokenRepository.SaveChangesAsync();

        return token;
    }

    public async Task<bool> ValidatePasswordResetTokenAsync(string userId, string token)
    {
        PasswordResetToken? resetToken = await _tokenRepository.FindAsync(userId, token);

        if (resetToken is null)
            return false;

        return !resetToken.IsUsed && resetToken.ExpiresAt > DateTime.UtcNow;
    }

    public async Task InvalidatePasswordResetTokenAsync(string userId, string token)
    {
        PasswordResetToken? resetToken = await _tokenRepository.FindAsync(userId, token);

        if (resetToken is not null)
        {
            resetToken.IsUsed = true;
            await _tokenRepository.SaveChangesAsync();
        }
    }
}
