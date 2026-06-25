using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Project2030.Application.DTOs.Auth;
using Project2030.Application.Interfaces;
using Project2030.Application.Interfaces.Repositories;
using Project2030.Domain.Entities;

namespace Project2030.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IAccessLogService _accessLogService;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserRepository userRepository,
        ITokenService tokenService,
        IAccessLogService accessLogService,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _accessLogService = accessLogService;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, string ipAddress)
    {
        ApplicationUser? user = await _userRepository.FindByEmailAsync(request.Email);

        if (user is null)
        {
            await _accessLogService.RegisterLoginAsync("anonymous", ipAddress, false, "Usuario no encontrado");
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        if (user.IsBlocked)
        {
            await _accessLogService.RegisterLoginAsync(user.Id, ipAddress, false, "Usuario bloqueado");
            throw new UnauthorizedAccessException($"Usuario bloqueado: {user.BlockReason}");
        }

        bool passwordValid = await _userRepository.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
        {
            await _accessLogService.RegisterLoginAsync(user.Id, ipAddress, false, "Contraseña incorrecta");
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        IList<string> roles = await _userRepository.GetRolesAsync(user);
        string role = roles.FirstOrDefault() ?? "Usuario";

        int expiresInMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"]!);
        string token = _tokenService.GenerateJwtToken(user, role);

        await _accessLogService.RegisterLoginAsync(user.Id, ipAddress, true);

        return new LoginResponseDto
        {
            Token = token,
            FullName = user.FullName,
            Email = user.Email!,
            Role = role,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes)
        };
    }

    public async Task LogoutAsync(string userId, string ipAddress)
    {
        await _accessLogService.RegisterLogoutAsync(userId, ipAddress);
    }

    public async Task ForgotPasswordAsync(ForgotPasswordRequestDto request)
    {
        ApplicationUser? user = await _userRepository.FindByEmailAsync(request.Email);

        if (user is null)
            return;

        string token = await _tokenService.GeneratePasswordResetTokenAsync(user.Id);

        // TODO P3: Enviar por email
        Console.WriteLine($"[DEV] Reset token para {request.Email}: {token}");
    }

    public async Task ResetPasswordAsync(ResetPasswordRequestDto request)
    {
        if (request.NewPassword != request.ConfirmPassword)
            throw new ArgumentException("Las contraseñas no coinciden");

        ApplicationUser? user = await _userRepository.FindByEmailAsync(request.Email);
        if (user is null)
            throw new KeyNotFoundException("Usuario no encontrado");

        bool tokenValid = await _tokenService.ValidatePasswordResetTokenAsync(user.Id, request.Token);
        if (!tokenValid)
            throw new InvalidOperationException("Token inválido o expirado");

        IdentityResult removeResult = await _userRepository.RemovePasswordAsync(user);
        if (!removeResult.Succeeded)
            throw new InvalidOperationException("No se pudo actualizar la contraseña");

        IdentityResult addResult = await _userRepository.AddPasswordAsync(user, request.NewPassword);
        if (!addResult.Succeeded)
        {
            string errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
            throw new ArgumentException($"La nueva contraseña no cumple los requisitos: {errors}");
        }

        await _tokenService.InvalidatePasswordResetTokenAsync(user.Id, request.Token);
    }
}
