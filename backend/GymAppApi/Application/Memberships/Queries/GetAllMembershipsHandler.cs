using GymAppApi.Data;
using GymAppApi.Domain.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Memberships.Queries
{
    public class GetAllMembershipsHandler(GymAppDbContext _context) : IRequestHandler<GetAllMembershipsCommand,List<MembershipDto>>
    {
        public async Task<List<MembershipDto>> Handle(GetAllMembershipsCommand command, CancellationToken ct)
        {
            var memberships = await _context.Memberships.Where(x => x.NullifiedAt == null && x.EndDate > DateTime.UtcNow).OrderByDescending(x => x.StartDate).Include(x => x.Gym).ToListAsync(ct);

            var membershipsDtos = memberships.Select(m => new MembershipDto
            {
                Id = m.Id,
                GymId = m.GymId,
                GymName = m.Gym.Name,
                Type = m.Type,
                StartDate = m.StartDate,
                EndDate = m.EndDate,
                SessionsLeft = m.SessionsLeft,
                NullifiedAt = m.NullifiedAt,
                IsActive = m.IsActive
            }).ToList();

            return membershipsDtos;
        }
    }
}
