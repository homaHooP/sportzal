using GymAppApi.Application.Common;
using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Bookings.Queries
{
    public class GetClientBookingsCommand : IRequest<List<BookingDto>>, IClientOwnedRequest
    {
        public Guid ClientId { get; set; }

        public Guid GetOwnerId() => ClientId;
    }
}
