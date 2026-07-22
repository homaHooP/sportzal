using FluentValidation.Results;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Data;
using GymAppApi.Domain.Constants;
using GymAppApi.Domain.Enums;
using GymAppApi.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Bookings.Commands
{
    public class MarkAttendanceHandler(GymAppDbContext _context, CurrentUserService _cus) : IRequestHandler<MarkAttendanceCommand>
    {
        public async Task Handle(MarkAttendanceCommand command, CancellationToken ct)
        {
            var booking = await _context.Bookings
                .Include(b => b.Session).ThenInclude(s => s.Trainer)
                .FirstOrDefaultAsync(b => b.Id == command.BookingId, ct);

            if (booking == null) { throw new NotFoundException("Booking", command.BookingId); }

            var isSessionTrainer = booking.Session.Trainer.UserId == _cus.UserId;

            if (!isSessionTrainer && !_cus.IsInRole(RoleNames.HeadManager))
            {
                if (_cus.IsInRole(RoleNames.Manager))
                {
                    var managesThisGym = await _context.UserGymManagers
                        .AnyAsync(m => m.UserId == _cus.UserId && m.GymId == booking.Session.GymId, ct);

                    if (!managesThisGym)
                    {
                        throw new ForbiddenAccessException("You don't manage the gym this booking belongs to");
                    }
                }
                else
                {
                    throw new ForbiddenAccessException("Only the session's trainer can mark attendance");
                }
            }

            if (booking.Status != BookingStatus.Booked)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Booking", $"Cannot mark attendance for a booking with status {booking.Status}") });
            }

            if (booking.Session.StartTime > DateTime.UtcNow)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Session", "Cannot mark attendance before the session has started") });
            }

            booking.Status = command.Status;
            await _context.SaveChangesAsync(ct);
        }
    }
}