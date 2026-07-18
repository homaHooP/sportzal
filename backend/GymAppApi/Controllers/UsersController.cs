using GymAppApi.Application.Users.Commands;
using GymAppApi.Application.Users.Queries;
using GymAppApi.Domain.DTO;
using GymAppApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GymAppApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthResultDto>> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken)
        {
            var tokens = await _mediator.Send(command,cancellationToken);
            return Ok(tokens);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthResultDto>> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
        {
            var tokens = await _mediator.Send(command, cancellationToken);
            return Ok(tokens);
        }

        [HttpGet("")]
        [Authorize(Policy = "RequireManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] GetUsersCommand command, CancellationToken cancellationToken)
        {
            var users = await _mediator.Send(command, cancellationToken);
            return Ok(users);
        }

        [HttpGet("{Id:Guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDetailsDto>> GetUserById([FromRoute] Guid Id, CancellationToken cancellationToken)
        {
            var command = new GetUserByIdCommand { Id = Id };
            var user = await _mediator.Send(command, cancellationToken);
            return Ok(user);
        }

        [HttpPost("giverole")]
        [Authorize(Policy = "RequireManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDetailsDto>> GiveUserRole([FromBody] GiveUserRoleCommand command, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(command, cancellationToken);
            return Ok(user);
        }

        [HttpDelete("{UserId:guid}")]
        [Authorize(Policy = "RequireManager")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid UserId, CancellationToken cancellationToken)
        {
            var command = new DeleteUserCommand { UserId = UserId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{UserId:guid}/roles/{Role}")]
        [Authorize(Policy = "RequireManager")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteRole([FromRoute] Guid UserId, [FromRoute] string Role, CancellationToken cancellationToken) { 
            var command = new DeleteUserRoleCommand { UserId = UserId, Role = Role };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPost("additionalinfo")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDetailsDto>> setFurtherInfo([FromBody] SetAdditionalInfoCommand command, CancellationToken ct)
        {
            var userDetails = await _mediator.Send(command, ct);
            return Ok(userDetails);
        }
    }
}
