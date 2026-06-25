using Project2030.Application.DTOs.Users;

namespace Project2030.Application.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto request);
    Task<UserResponseDto> GetUserByIdAsync(string userId);
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
}
