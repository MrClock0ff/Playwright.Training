using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Playwright.Training.Api.Services;
using Playwright.Training.Domain.DTOs;

namespace Playwright.Training.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("/api/v{version:apiVersion}/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUserService _userService;

    public UsersController(
        ILogger<UsersController> logger,
        IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [ActionName(nameof(GetUserAsync))]
    [HttpGet("{userId}")]
    [EndpointDescription("Gets specified user account.")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "View Users")]
    public async Task<ActionResult<UserDto>> GetUserAsync(
        [FromRoute] Guid userId, 
        CancellationToken cancellationToken)
    {
        UserDto? userDto = await _userService.GetUserByIdAsync(userId, cancellationToken);

        if (userDto == null)
        {
            return NotFound();
        }
        
        return Ok(userDto);
    }

    [HttpGet]
    [EndpointDescription("Gets all users accounts.")]
    [ProducesResponseType<IEnumerable<UserDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "View Users")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersAsync(CancellationToken cancellationToken)
    {
        IEnumerable<UserDto> userDtos = await _userService.GetAllUsers(cancellationToken);
        return Ok(userDtos);
    }

    [HttpPost]
    [EndpointDescription("Creates new user account.")]
    [ProducesResponseType<UserDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "Create Users")]
    public async Task<ActionResult<UserDto>> CreateUserAsync(
        [FromBody] CreateUserDto user, 
        CancellationToken cancellationToken)
    {
        UserDto userDto = await _userService.CreateUserAsync(user, cancellationToken);
        return CreatedAtAction(nameof(GetUserAsync), new { userId = userDto.Id}, userDto);
    }

    [HttpDelete("{userId}")]
    [EndpointDescription("Deletes specified user account.")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Delete Users")]
    public async Task<ActionResult<UserDto>> DeleteUserAsync(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        UserDto? userDto = await _userService.DeleteUserWithIdAsync(userId, cancellationToken);

        if (userDto == null)
        {
            return NotFound();
        }
        
        return Ok(userDto);
    }

    [HttpPatch("{userId}")]
    [EndpointDescription("Updates specified user account.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Edit Users")]
    public async Task<ActionResult<UserDto>> UpdateUserAsync(
        [FromRoute] Guid userId,
        [FromBody] JsonPatchDocument<UserDto> patchDocument,
        CancellationToken cancellationToken)
    {
        UserDto? userDto = await _userService.UpdateUserWithIdAsync(userId, patchDocument, cancellationToken);

        if (userDto == null)
        {
            return NotFound();
        }
        
        return Ok(userDto);
    }
}