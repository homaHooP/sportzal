using GymAppApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GymAppApi.Domain.DTO;
using GymAppApi.Application.Memberships.Queries;
using GymAppApi.Application.Memberships.Commands;

namespace GymAppApi.Controllers
{
    [Route("api/memberships")]
    [ApiController]
    public class MembershipsController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = "RequireHeadManager")]
        public async Task<ActionResult<List<MembershipDto>>> GetAllMemberships(CancellationToken ct)
        {
            var command = new GetAllMembershipsCommand();
            var memberships = await _mediator.Send(command, ct);
            return Ok(memberships);
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        public async Task<ActionResult<MembershipDto>> Assign([FromBody] AssignMembershipCommand command, CancellationToken ct)
        {
            var membership = await _mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetByClient), new { ClientId = command.ClientId }, membership);
        }

        [HttpDelete("{Id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        public async Task<IActionResult> Nullify([FromRoute] Guid Id, CancellationToken ct)
        {
            await _mediator.Send(new NullifyMembershipCommand { MembershipId = Id }, ct);
            return NoContent();
        }

        [HttpGet("clients/{ClientId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<ClientMembershipsDto>> GetByClient([FromRoute] Guid ClientId, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetClientMembershipsCommand { ClientId = ClientId }, ct);
            return Ok(result);
        }
    }
}
