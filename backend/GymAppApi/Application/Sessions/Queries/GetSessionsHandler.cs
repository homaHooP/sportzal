using GymAppApi.Data;
using GymAppApi.Domain.DTO;
using GymAppApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Sessions.Queries
{
    public class GetSessionsHandler(GymAppDbContext _context) : IRequestHandler<GetSessionsCommand,List<SessionDto>>
    {
        public async Task<List<SessionDto>> Handle (GetSessionsCommand command, CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            return await _context.Sessions
                .Where(x => x.CancelledAt == null && x.EndTime > DateTime.UtcNow)
                .OrderByDescending(x => x.StartTime)
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
