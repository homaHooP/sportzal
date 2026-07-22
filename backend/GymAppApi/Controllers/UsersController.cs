using GymAppApi.Application.Users.Commands;
using GymAppApi.Application.Users.Commands.Promotions;
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
        [Authorize(Policy = "RequireHeadManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] GetUsersCommand command, CancellationToken cancellationToken)
        {
            var users = await _mediator.Send(command, cancellationToken);
            return Ok(users);
        }

        [HttpGet("deactivated")]
        [Authorize(Policy = "RequireHeadManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetDeactivatedUsers([FromQuery] GetDeactivatedUsersCommand command, CancellationToken cancellationToken)
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

        [HttpDelete("{UserId:guid}")]
        [Authorize(Policy = "RequireManager")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid UserId, CancellationToken cancellationToken)
        {
            var command = new DeleteUserCommand { UserId = UserId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        #region Promote
        [HttpPost("clienttotrainer/{ClientId:guid}")]
        [Authorize(Policy = "RequireManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDetailsDto>> PromoteClientToTrainer([FromBody] PromoteClientToTrainerCommand command, [FromRoute] Guid ClientId, CancellationToken cancellationToken)
        {
            command.ClientId = ClientId;
            var trainer = await _mediator.Send(command, cancellationToken);
            return Ok(trainer);
        }

        [HttpPost("clienttomanager/{ClientId:guid}")]
        [Authorize(Policy = "RequireHeadManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDetailsDto>> PromoteClientToManager([FromBody] PromoteClientToManagerCommand command, [FromRoute] Guid ClientId, CancellationToken cancellationToken)
        {
            command.ClientId = ClientId;
            var manager = await _mediator.Send(command, cancellationToken);
            return Ok(manager);
        }

        [HttpPost("clienttoheadmanager/{ClientId:guid}")]
        [Authorize(Policy = "RequireHeadManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDetailsDto>> PromoteClientToHeadManager([FromBody] PromoteClientToHeadManagerCommand command, [FromRoute] Guid ClientId, CancellationToken cancellationToken)
        {
            command.ClientId = ClientId;
            var headmanager = await _mediator.Send(command, cancellationToken);
            return Ok(headmanager);
        }

        [HttpPost("trainertomanager/{TrainerId:guid}")]
        [Authorize(Policy = "RequireHeadManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDetailsDto>> PromoteTrainerToManager([FromBody] PromoteTrainerToManagerCommand command, [FromRoute] Guid TrainerId, CancellationToken cancellationToken)
        {
            command.TrainerId = TrainerId;
            var manager = await _mediator.Send(command, cancellationToken);
            return Ok(manager);
        }

        [HttpPost("managertoheadmanager/{ManagerId:guid}")]
        [Authorize(Policy = "RequireHeadManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDetailsDto>> PromoteManagerToHeadManager([FromBody] PromoteManagerToHeadManagerCommand command, [FromRoute] Guid ManagerId, CancellationToken cancellationToken)
        {
            command.ManagerId = ManagerId;
            var manager = await _mediator.Send(command, cancellationToken);
            return Ok(manager);
        }
        #endregion

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
