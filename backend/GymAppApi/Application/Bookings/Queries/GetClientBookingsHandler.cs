using GymAppApi.Data;
using GymAppApi.Domain.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Services;
using GymAppApi.Domain.Constants;

namespace GymAppApi.Application.Bookings.Queries
{
    public class GetClientBookingsHandler(GymAppDbContext _context, CurrentUserService _cus) : IRequestHandler<GetClientBookingsCommand,List<BookingDto>>
    {
        public async Task<List<BookingDto>> Handle (GetClientBookingsCommand command, CancellationToken ct)
        {
            var client = await _context.UserClients.Include(x => x.Bookings).FirstOrDefaultAsync(c => c.UserId == command.ClientId, ct);
            if (client == null) { throw new NotFoundException("Client", command.ClientId); }

            return await _context.Bookings
                .OrderByDescending(b => b.BookedAt)
                .Where(x => x.ClientId == client.UserId)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    ClientId = b.ClientId,
                    SessionId = b.SessionId,
                    ClientName = b.Client.User.FullName,
                    TrainerName = b.Session.Trainer.User.FullName,
                    SessionStartTime = b.Session.StartTime,
                    SessionEndTime = b.Session.EndTime,
                    Status = b.Status,
                    BookedAt = b.BookedAt
                })
                .ToListAsync(ct);
        }
    }
}
