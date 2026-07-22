using GymAppApi.Domain.Enums;
using MediatR;

namespace GymAppApi.Application.Bookings.Commands
{
    public class MarkAttendanceCommand : IRequest
    {
        public Guid BookingId { get; set; }
        public BookingStatus Status { get; set; }
    }
}