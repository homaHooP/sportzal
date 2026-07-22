using FluentValidation.Results;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Data;
using GymAppApi.Domain.Constants;
using GymAppApi.Domain.Enums;
using GymAppApi.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Sessions.Commands
{
    public class CancelSessionHandler(GymAppDbContext _context, CurrentUserService _cus) : IRequestHandler<CancelSessionCommand>
    {
        public async Task Handle(CancelSessionCommand command, CancellationToken ct)
        {
            var session = await _context.Sessions
                .Include(s => s.Trainer)
                .Include(s => s.Bookings)
                .FirstOrDefaultAsync(s => s.Id == command.SessionId, ct);

            if (session == null) { throw new NotFoundException("Session", command.SessionId); }

            if (session.CancelledAt != null)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Session", "Session is already cancelled") });
            }

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
                    throw new ForbiddenAccessException("You can only cancel your own sessions");
                }
            }

            if (session.StartTime < DateTime.UtcNow)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Session", "Session is ongoing or finished") });
            }

            session.CancelledAt = DateTime.UtcNow;

            foreach (var booking in session.Bookings.Where(b => b.Status == BookingStatus.Booked))
            {
                booking.Status = BookingStatus.Cancelled;
            }

            await _context.SaveChangesAsync(ct);
        }
    }
}