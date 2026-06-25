using Project2030.Application.DTOs.Profile;
using Project2030.Application.DTOs.Users;

namespace Project2030.Application.Interfaces;

public interface IProfileService
{
    Task<UserResponseDto> GetProfileAsync(string userId);
    Task<UserResponseDto> UpdateProfileAsync(string userId, UpdateProfileRequestDto request);
    Task ChangePasswordAsync(string userId, ChangePasswordRequestDto request);
}
