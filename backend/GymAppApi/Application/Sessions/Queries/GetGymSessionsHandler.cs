using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Data;
using GymAppApi.Domain.DTO;
using GymAppApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Sessions.Queries
{
    public class GetGymSessionsHandler(GymAppDbContext _context) : IRequestHandler<GetGymSessionsCommand, List<SessionDto>>
    {
        public async Task<List<SessionDto>> Handle(GetGymSessionsCommand command, CancellationToken ct)
        {
            var gymExists = await _context.Gyms.AnyAsync(g => g.Id == command.GymId, ct);
            if (!gymExists) { throw new NotFoundException("Gym", command.GymId); }

            var now = DateTime.UtcNow;

            return await _context.Sessions
                .Where(x => x.GymId == command.GymId && x.CancelledAt == null && x.EndTime > now)
                .OrderBy(x => x.StartTime)
                .Select(x => new SessionDto
                {
                    Id = x.Id,
                    GymId = x.GymId,
                    GymName = x.Gym.Name,
                    TrainerId = x.TrainerId,
                    TrainerName = x.Trainer.User.FullName,
                    Type = x.Type,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    MaxParticipants = x.MaxParticipants,
                    CancelledAt = x.CancelledAt,
                    IsAvailable = x.CancelledAt == null && x.StartTime > now && x.Bookings.Count(b => b.Status == BookingStatus.Booked) < x.MaxParticipants
                })
                .ToListAsync(ct);
        }
    }
}