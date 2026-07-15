using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GymAppApi.Domain.Models;
using GymAppApi.Application.Gyms.Commands;

namespace GymAppApi.Controllers
{
    [Route("api/gyms")]
    [ApiController]
    public class GymController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Gym>> Create([FromBody] CreateGymCommand command, CancellationToken cancellationToken)
        {
            var gym = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(Create), new { id = gym.Id }, gym);
        }
    }
}
