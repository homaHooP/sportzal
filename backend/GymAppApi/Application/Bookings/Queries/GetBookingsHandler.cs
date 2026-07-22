using GymAppApi.Data;
using GymAppApi.Domain.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Bookings.Queries
{
    public class GetBookingsHandler(GymAppDbContext _context) : IRequestHandler<GetBookingsCommand, List<BookingDto>>
    {
        public async Task<List<BookingDto>> Handle(GetBookingsCommand command, CancellationToken ct)
        {
            return await _context.Bookings
                .OrderByDescending(b => b.BookedAt)
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