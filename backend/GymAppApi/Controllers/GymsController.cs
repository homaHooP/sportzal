using GymAppApi.Application.Gyms.Commands;
using GymAppApi.Application.Users.Queries;
using GymAppApi.Domain.Models;
using GymAppApi.Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GymAppApi.Application.Gyms.Queries;

namespace GymAppApi.Controllers
{
    [Route("api/gyms")]
    [ApiController]
    public class GymsController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = "RequireHeadManager")]
        public async Task<ActionResult<GymDetailsDto>> Create([FromBody] CreateGymCommand command, CancellationToken cancellationToken)
        {
            var gym = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetGymById), new { id = gym.Id }, gym);
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GymDto>>> GetGyms([FromQuery] GetGymsCommand command, CancellationToken cancellationToken)
        {
            var gyms = await _mediator.Send(command, cancellationToken);
            return Ok(gyms);
        }

        [HttpGet("{Id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = "RequireManager")]
        public async Task<ActionResult<GymDetailsDto>> GetGymById([FromRoute] Guid Id, CancellationToken cancellationToken)
        {
            var command = new GetGymByIdCommand { GymId = Id };
            var gym = await _mediator.Send(command, cancellationToken);
            return Ok(gym);
        }

        [HttpPut("{Id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = "RequireManager")]
        public async Task<ActionResult<GymDetailsDto>> EditGym([FromRoute] Guid Id, [FromBody] EditGymCommand command, CancellationToken cancellationToken)
        {
            command.GymId = Id;
            var gym = await _mediator.Send(command, cancellationToken);
            return Ok(gym);
        }

        [HttpDelete("{Id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = "RequireHeadManager")]
        public async Task<IActionResult> DeleteGym([FromRoute] Guid Id, CancellationToken cancellationToken)
        {
            var command = new DeleteGymCommand { GymId = Id };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        //Немає AppendManagerList, бо при створенні менеджера всеодно потрібно вказувати GymId, що не дасть менеджеру бути без Gym

        [HttpDelete("{Id:guid}/managers/{ManagerId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = "RequireHeadManager")]
        public async Task<IActionResult> DeleteManager([FromRoute] Guid Id, [FromRoute] Guid ManagerId, CancellationToken cancellationToken)
        {
            var command = new DeleteGymManagerCommand { GymId = Id, ManagerId = ManagerId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}

//gyms/4712bca7-b40f-4b12-98ac-8641f9286c6f/managers/bb1d6765-f75a-4cc8-15aa-08dee7030751