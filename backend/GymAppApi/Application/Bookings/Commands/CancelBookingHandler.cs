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
    public class CancelBookingHandler(GymAppDbContext _context, CurrentUserService _cus) : IRequestHandler<CancelBookingCommand>
    {
        public async Task Handle(CancelBookingCommand command, CancellationToken ct)
        {
            var booking = await _context.Bookings
                .Include(b => b.Session).ThenInclude(s => s.Trainer)
                .FirstOrDefaultAsync(b => b.Id == command.BookingId, ct);

            if (booking == null) { throw new NotFoundException("Booking", command.BookingId); }

            var isOwner = booking.ClientId == _cus.UserId;
            var isSessionTrainer = booking.Session.Trainer.UserId == _cus.UserId;

            if (!isOwner && !isSessionTrainer && !_cus.IsInRole(RoleNames.HeadManager))
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
                    throw new ForbiddenAccessException("You can only cancel your own bookings");
                }
            }

            if (booking.Status == BookingStatus.Cancelled)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Booking", "Booking is already cancelled") });
            }

            booking.Status = BookingStatus.Cancelled;

            // Повертаємо сесію на абонемент, якщо абонемент ще активний (не анульований і не прострочений).
            var membership = await _context.Memberships.FirstOrDefaultAsync(m =>
                m.ClientId == booking.ClientId &&
                m.GymId == booking.Session.GymId &&
                m.NullifiedAt == null &&
                m.EndDate > DateTime.UtcNow, ct);

            if (membership != null)
            {
                membership.SessionsLeft += 1;
            }

            await _context.SaveChangesAsync(ct);
        }
    }
}