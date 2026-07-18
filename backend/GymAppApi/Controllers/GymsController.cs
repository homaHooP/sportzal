using GymAppApi.Application.Gyms.Commands;
using GymAppApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymAppApi.Controllers
{
    [Route("api/gyms")]
    [ApiController]
    [Authorize (Policy = "RequireHeadManager")]
    public class GymsController(IMediator _mediator) : ControllerBase
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
