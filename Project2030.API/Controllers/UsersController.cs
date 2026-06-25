using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project2030.Application.DTOs.Users;
using Project2030.Application.Interfaces;

namespace Project2030.API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Roles = "SuperAdministrador,Administrador")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto request)
    {
        UserResponseDto user = await _userService.CreateUserAsync(request);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllUsers()
    {
        IEnumerable<UserResponseDto> users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(string id)
    {
        UserResponseDto user = await _userService.GetUserByIdAsync(id);
        return Ok(user);
    }
}
