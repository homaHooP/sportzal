using GymAppApi.Data;
using GymAppApi.Domain.DTO;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace GymAppApi.Application.Gyms.Queries
{
    public class GetGymsHandler(GymAppDbContext _context)
    : IRequestHandler<GetGymsCommand, List<GymDto>>
    {
        public async Task<List<GymDto>> Handle(GetGymsCommand command, CancellationToken ct)
        {
            var gyms = await _context.Gyms
                .Select(g => new GymDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Address = g.Address,
                }).ToListAsync(ct);
            return gyms;
        }
    }
}
