using GymAppApi.Application.Common;
using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Bookings.Commands
{
    public class BookASessionCommand : IRequest<BookingDto>, IClientOwnedRequest
    {
        public Guid SessionId { get; set; }
        public Guid ClientId { get; set; }

        public Guid GetOwnerId() => ClientId;
    }
}