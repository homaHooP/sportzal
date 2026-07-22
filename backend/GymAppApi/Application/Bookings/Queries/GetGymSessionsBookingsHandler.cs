using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Data;
using GymAppApi.Domain.Constants;
using GymAppApi.Domain.DTO;
using GymAppApi.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Bookings.Queries
{
    public class GetGymSessionsBookingsHandler(GymAppDbContext _context, CurrentUserService _cus) : IRequestHandler<GetGymSessionsBookingsCommand, List<BookingDto>>
    {
        public async Task<List<BookingDto>> Handle(GetGymSessionsBookingsCommand command, CancellationToken ct)
        {
            var gymExists = await _context.Gyms.AnyAsync(g => g.Id == command.GymId, ct);
            if (!gymExists) { throw new NotFoundException("Gym", command.GymId); }

            // RequireManager-policy пропускає Manager/HeadManager глобально,
            // тут звужуємо Manager до його власного залу.
            if (!_cus.IsInRole(RoleNames.HeadManager))
            {
                var managesThisGym = await _context.UserGymManagers
                    .AnyAsync(m => m.UserId == _cus.UserId && m.GymId == command.GymId, ct);

                if (!managesThisGym)
                {
                    throw new ForbiddenAccessException("You don't manage this gym");
                }
            }

            return await _context.Bookings
                .Where(b => b.Session.GymId == command.GymId)
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