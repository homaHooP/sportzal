using GymAppApi.Domain.Models;
using GymAppApi.Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GymAppApi.Application.Bookings.Queries;
using GymAppApi.Application.Bookings.Commands;

namespace GymAppApi.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingsController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = "RequireHeadManager")]
        public async Task<ActionResult<List<BookingDto>>> GetBookings(CancellationToken ct)
        {
            var command = new GetBookingsCommand();
            var bookings = await _mediator.Send(command, ct);
            return Ok(bookings);
        }

        [HttpGet("sessions/{SessionId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = "RequireTrainer")]
        public async Task<ActionResult<List<BookingDto>>> GetSessionBookings([FromRoute] Guid SessionId ,CancellationToken ct)
        {
            var command = new GetSessionBookingsCommand { SessionId = SessionId };
            var bookings = await _mediator.Send(command, ct);
            return Ok(bookings);
        }

        [HttpGet("gyms/{GymId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = "RequireManager")]
        public async Task<ActionResult<List<BookingDto>>> GetGymSessionsBookings([FromRoute] Guid GymId, CancellationToken ct)
        {
            var command = new GetGymSessionsBookingsCommand { GymId = GymId };
            var bookings = await _mediator.Send(command, ct);
            return Ok(bookings);
        }

        [HttpGet("clients/{ClientId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<List<BookingDto>>> GetClientBookings([FromRoute] Guid ClientId, CancellationToken ct)
        {
            var command = new GetClientBookingsCommand { ClientId = ClientId };
            var bookings = await _mediator.Send(command, ct);
            return Ok(bookings);
        }

        [HttpPost("{SessionId:guid}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        public async Task<ActionResult<BookingDto>> BookASession([FromRoute] Guid SessionId, [FromBody] BookASessionCommand command, CancellationToken ct)
        {
            command.SessionId = SessionId;
            var booking = await _mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetSessionBookings), new { SessionId = booking.SessionId }, booking);
        }

        [HttpDelete("{BookingId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        public async Task<IActionResult> CancellBooking([FromRoute] Guid BookingId, CancellationToken ct)
        {
            var command = new CancelBookingCommand { BookingId = BookingId };
            await _mediator.Send(command, ct);
            return NoContent();
        }

        [HttpPatch("{BookingId:guid}/attendance")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = "RequireTrainer")]
        public async Task<IActionResult> MarkAttendance(
            [FromRoute] Guid BookingId, [FromBody] MarkAttendanceCommand command, CancellationToken ct)
        {
            command.BookingId = BookingId;
            await _mediator.Send(command, ct);
            return NoContent();
        }
    }
}
