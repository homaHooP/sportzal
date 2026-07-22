using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Bookings.Queries
{
    public class GetGymSessionsBookingsCommand : IRequest<List<BookingDto>>
    {
        public Guid GymId { get; set; }
    }
}