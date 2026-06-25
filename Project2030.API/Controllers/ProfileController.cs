using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project2030.Application.DTOs.Profile;
using Project2030.Application.DTOs.Users;
using Project2030.Application.Interfaces;

namespace Project2030.API.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetProfile()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        UserResponseDto profile = await _profileService.GetProfileAsync(userId);
        return Ok(profile);
    }

    [HttpPut]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequestDto request)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        UserResponseDto profile = await _profileService.UpdateProfileAsync(userId, request);
        return Ok(profile);
    }

    [HttpPost("change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _profileService.ChangePasswordAsync(userId, request);
        return Ok(new { message = "Contraseña cambiada correctamente" });
    }
}
