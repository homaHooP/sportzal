using MediatR;

namespace GymAppApi.Application.Bookings.Commands
{
    public class CancelBookingCommand : IRequest
    {
        public Guid BookingId { get; set; }
    }
}