using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using GymAppApi.Domain.DTO;
using GymAppApi.Application.Sessions.Commands;
using GymAppApi.Application.Sessions.Queries;

namespace GymAppApi.Controllers
{
    [Route("api/sessions")]
    [ApiController]
    public class SessionsController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = "RequireHeadManager")]
        public async Task<ActionResult<List<SessionDto>>> GetSessions(CancellationToken ct)
        {
            var command = new GetSessionsCommand();
            var sessions = await _mediator.Send(command, ct);
            return Ok(sessions);
        }

        [HttpGet("{GymId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<List<SessionDto>>> GetGymSessions([FromRoute] Guid GymId,CancellationToken ct)
        {
            var command = new GetGymSessionsCommand { GymId = GymId };
            var sessions = await _mediator.Send(command, ct);
            return Ok(sessions);
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = "RequireManager")]
        public async Task<ActionResult<SessionDto>> CreateSession([FromBody] CreateSessionCommand command, CancellationToken ct)
        {
            var session = await _mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetGymSessions), new { GymId = session.GymId }, session);
        } 

        [HttpDelete("{SessionId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = "RequireTrainer")]
        public async Task<IActionResult> CancelSession([FromRoute] Guid SessionId, CancellationToken ct)
        {
            var command = new CancelSessionCommand { SessionId = SessionId };
            await _mediator.Send(command, ct);
            return NoContent();
        }
    }
}
