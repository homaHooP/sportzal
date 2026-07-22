using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Bookings.Queries
{
    public class GetBookingsCommand : IRequest<List<BookingDto>>
    {
    }
}