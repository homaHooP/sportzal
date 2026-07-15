using GymAppApi.Application.Users.Commands;
using GymAppApi.Domain.DTO;
using GymAppApi.Domain.Models;
using MediatR;
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
    }
}
