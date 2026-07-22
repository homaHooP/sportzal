using FluentValidation.Results;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Data;
using GymAppApi.Domain.Constants;
using GymAppApi.Domain.DTO;
using GymAppApi.Domain.Models;
using GymAppApi.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Sessions.Commands
{
    public class CreateSessionHandler(GymAppDbContext _context, CurrentUserService _cus)
        : IRequestHandler<CreateSessionCommand, SessionDto>
    {
        public async Task<SessionDto> Handle(CreateSessionCommand command, CancellationToken ct)
        {
            var gym = await _context.Gyms.FirstOrDefaultAsync(g => g.Id == command.GymId, ct);
            if (gym == null) { throw new NotFoundException("Gym", command.GymId); }
            if (gym.DeletedAt != null)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Gym", "Gym is deleted") });
            }

            if (!_cus.IsInRole(RoleNames.HeadManager))
            {
                var managesThisGym = await _context.UserGymManagers
                    .AnyAsync(m => m.UserId == _cus.UserId && m.GymId == command.GymId, ct);

                if (!managesThisGym)
                {
                    throw new ForbiddenAccessException("You don't manage this gym");
                }
            }

            var trainer = await _context.UserTrainers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.UserId == command.TrainerId, ct);

            if (trainer == null) { throw new NotFoundException("Trainer", command.TrainerId); }

            // Тренер може мати сесії в різних залах, тому перевіряємо конфлікт за TrainerId без фільтра по GymId.
            var hasOverlap = await _context.Sessions.AnyAsync(s =>
                s.TrainerId == command.TrainerId &&
                s.CancelledAt == null &&
                s.StartTime < command.EndTime &&
                command.StartTime < s.EndTime, ct);

            if (hasOverlap)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Trainer", "Trainer already has a session that overlaps with this time range") });
            }

            var session = new Session
            {
                GymId = command.GymId,
                TrainerId = command.TrainerId,
                Type = command.Type,
                StartTime = command.StartTime,
                EndTime = command.EndTime,
                MaxParticipants = command.MaxParticipants
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync(ct);

            return new SessionDto
            {
                Id = session.Id,
                GymId = gym.Id,
                GymName = gym.Name,
                TrainerId = trainer.UserId,
                TrainerName = trainer.User.FullName,
                Type = session.Type,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                MaxParticipants = session.MaxParticipants,
                CancelledAt = null,
                IsAvailable = true
            };
        }
    }
}