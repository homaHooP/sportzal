using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Data;
using GymAppApi.Domain.Constants;
using GymAppApi.Domain.DTO;
using GymAppApi.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Bookings.Queries
{
    public class GetSessionBookingsHandler(GymAppDbContext _context, CurrentUserService _cus)
        : IRequestHandler<GetSessionBookingsCommand, List<BookingDto>>
    {
        public async Task<List<BookingDto>> Handle(GetSessionBookingsCommand command, CancellationToken ct)
        {
            var session = await _context.Sessions
                .Include(s => s.Trainer)
                .FirstOrDefaultAsync(s => s.Id == command.SessionId, ct);

            if (session == null) { throw new NotFoundException("Session", command.SessionId); }

            var isOwnSession = session.Trainer.UserId == _cus.UserId;

            if (!isOwnSession && !_cus.IsInRole(RoleNames.HeadManager))
            {
                if (_cus.IsInRole(RoleNames.Manager))
                {
                    var managesThisGym = await _context.UserGymManagers
                        .AnyAsync(m => m.UserId == _cus.UserId && m.GymId == session.GymId, ct);

                    if (!managesThisGym)
                    {
                        throw new ForbiddenAccessException("You don't manage the gym this session belongs to");
                    }
                }
                else
                {
                    throw new ForbiddenAccessException("You can only view bookings for your own sessions");
                }
            }

            return await _context.Bookings
                .Where(b => b.SessionId == command.SessionId)
                .OrderByDescending(b => b.BookedAt)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    ClientId = b.ClientId,
                    SessionId = b.SessionId,
                    ClientName = b.Client.User.FullName,
                    TrainerName = session.Trainer.User.FullName,
                    SessionStartTime = session.StartTime,
                    SessionEndTime = session.EndTime,
                    Status = b.Status,
                    BookedAt = b.BookedAt
                })
                .ToListAsync(ct);
        }
    }
}