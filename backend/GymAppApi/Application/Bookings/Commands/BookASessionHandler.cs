using FluentValidation.Results;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Data;
using GymAppApi.Domain.Constants;
using GymAppApi.Domain.DTO;
using GymAppApi.Domain.Enums;
using GymAppApi.Domain.Models;
using GymAppApi.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Bookings.Commands
{
    public class BookASessionHandler(GymAppDbContext _context, CurrentUserService _cus)
        : IRequestHandler<BookASessionCommand, BookingDto>
    {
        public async Task<BookingDto> Handle(BookASessionCommand command, CancellationToken ct)
        {
            var client = await _context.UserClients.Include(x=>x.User).FirstOrDefaultAsync(c => c.UserId == command.ClientId, ct);
            if (client == null) { throw new NotFoundException("Client", command.ClientId); }
            if (client.User.WasDeactivated != null)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("User", "Deactivated user cannot book sessions") });
            }

            var session = await _context.Sessions
                .Include(s => s.Trainer).ThenInclude(t=>t.User)
                .Include(s => s.Gym)
                .FirstOrDefaultAsync(s => s.Id == command.SessionId, ct);

            if (session == null) { throw new NotFoundException("Session", command.SessionId); }

            if (session.CancelledAt != null)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Session", "This session is cancelled") });
            }

            if (session.StartTime <= DateTime.UtcNow)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Session", "Cannot book a session that has already started") });
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(ct);

            // Абонемент клієнта має належати саме залу цієї сесії й мати вільні сесії.
            var membership = await _context.Memberships.FirstOrDefaultAsync(m =>
                m.ClientId == command.ClientId &&
                m.NullifiedAt == null &&
                m.EndDate > DateTime.UtcNow, ct);

            if (membership == null)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Membership", "You don't have an active membership") });
            }

            if (membership.GymId != session.GymId)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Membership", "Your active membership doesn't belong to this gym") });
            }

            if (_cus.IsInRole(RoleNames.Manager))
            {
                var isValidManager = await _context.UserGymManagers.Where(x => x.UserId == _cus.UserId && x.GymId == membership.GymId).AnyAsync(ct);
                if (!isValidManager)
                {
                    throw new ValidationException(new List<ValidationFailure>
                    {new("Manager", "You can't book sessions for clients that aren't in your gym") });
                }
            }

            if (membership.SessionsLeft <= 0)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Membership", "You don't have sessions left on your membership") });
            }

            var alreadyBooked = await _context.Bookings.AnyAsync(b =>
                b.ClientId == command.ClientId &&
                b.SessionId == command.SessionId &&
                b.Status == BookingStatus.Booked, ct);

            if (alreadyBooked)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Session", "You already have an active booking for this session") });
            }

            var hasOverlappingBooking = await _context.Bookings.AnyAsync(b =>
                b.ClientId == command.ClientId &&
                b.Status == BookingStatus.Booked &&
                b.Session.CancelledAt == null &&
                b.Session.StartTime < session.EndTime &&
                session.StartTime < b.Session.EndTime, ct);

            if (hasOverlappingBooking)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Session", "You already have another booking that overlaps with this time range") });
            }

            var activeBookingsCount = await _context.Bookings.CountAsync(b =>
                b.SessionId == command.SessionId &&
                b.Status == BookingStatus.Booked, ct);

            if (activeBookingsCount >= session.MaxParticipants)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Session", "This session is fully booked") });
            }

            var booking = new Booking
            {
                ClientId = command.ClientId,
                SessionId = command.SessionId,
                Status = BookingStatus.Booked,
                BookedAt = DateTime.UtcNow
            };

            membership.SessionsLeft -= 1;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return new BookingDto
            {
                Id = booking.Id,
                ClientId = booking.ClientId,
                SessionId = booking.SessionId,
                ClientName = client.User.FullName,
                TrainerName = session.Trainer.User.FullName,
                SessionStartTime = session.StartTime,
                SessionEndTime = session.EndTime,
                Status = booking.Status,
                BookedAt = booking.BookedAt
            };
        }
    }
}