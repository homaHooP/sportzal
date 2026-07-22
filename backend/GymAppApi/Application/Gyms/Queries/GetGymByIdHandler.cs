using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Data;
using GymAppApi.Domain.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Gyms.Queries
{
    public class GetGymByIdHandler(GymAppDbContext _context) : IRequestHandler<GetGymByIdCommand, GymDetailsDto>
    {
        public async Task<GymDetailsDto> Handle(GetGymByIdCommand command, CancellationToken ct)
        {
            var gym = await _context.Gyms
                .Include(x => x.Managers).ThenInclude(m => m.User)
                .FirstOrDefaultAsync(x => x.Id == command.GymId, ct);

            if (gym == null) { throw new NotFoundException("Gym", command.GymId); }

            var sessionsCount = await _context.Sessions.CountAsync(s => s.GymId == gym.Id, ct);
            var membershipsCount = await _context.Memberships.CountAsync(m => m.GymId == gym.Id, ct);

            return new GymDetailsDto
            {
                Id = gym.Id,
                Name = gym.Name,
                Address = gym.Address,
                ManagerNames = gym.Managers
                    .Select(x => x.User?.Email ?? "Deactivated user")
                    .ToList(),
                SessionsCount = sessionsCount,
                MembershipsCount = membershipsCount,
                DeletedAt = gym.DeletedAt == null ? "Gym isn't deleted" : gym.DeletedAt.ToString()
            }; 
        }
    }
}
