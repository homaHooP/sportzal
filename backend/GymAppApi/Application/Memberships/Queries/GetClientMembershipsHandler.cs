using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Data;
using GymAppApi.Domain.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Memberships.Queries
{
    public class GetClientMembershipsHandler(GymAppDbContext _context): IRequestHandler<GetClientMembershipsCommand, ClientMembershipsDto>
    {
        public async Task<ClientMembershipsDto> Handle(GetClientMembershipsCommand command, CancellationToken ct)
        {
            var client = await _context.UserClients.FirstOrDefaultAsync(c => c.UserId == command.ClientId, ct);
            if (client == null) { throw new NotFoundException("Client", command.ClientId); }

            var memberships = await _context.Memberships
                .Include(m => m.Gym)
                .Where(m => m.ClientId == command.ClientId)
                .OrderByDescending(m => m.StartDate)
                .ToListAsync(ct);

            var now = DateTime.UtcNow;

            var dtos = memberships.Select(m => new MembershipDto
            {
                Id = m.Id,
                GymId = m.GymId,
                GymName = m.Gym?.Name ?? "Unknown gym",
                Type = m.Type,
                StartDate = m.StartDate,
                EndDate = m.EndDate,
                SessionsLeft = m.SessionsLeft,
                NullifiedAt = m.NullifiedAt,
                IsActive = m.NullifiedAt == null && m.EndDate > now
            }).ToList();

            return new ClientMembershipsDto
            {
                Active = dtos.FirstOrDefault(d => d.IsActive),
                History = dtos.Where(d => !d.IsActive).ToList()
            };
        }
    }
}