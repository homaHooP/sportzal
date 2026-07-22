using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Bookings.Queries
{
    public class GetSessionBookingsCommand : IRequest<List<BookingDto>>
    {
        public Guid SessionId { get; set; }
    }
}