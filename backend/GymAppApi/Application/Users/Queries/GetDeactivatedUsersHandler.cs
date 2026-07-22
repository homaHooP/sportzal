using GymAppApi.Data;
using GymAppApi.Domain.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Users.Queries
{
    public class GetDeactivatedUsersHandler(GymAppDbContext _context) : IRequestHandler<GetDeactivatedUsersCommand, IEnumerable<UserDto>>
    {
        public async Task<IEnumerable<UserDto>> Handle(GetDeactivatedUsersCommand command, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Where(u => u.WasDeactivated != null)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email ?? string.Empty,
                    FullName = u.FullName ?? string.Empty,
                    Roles = (from ur in u.UserRoles
                             join r in _context.Roles on ur.RoleId equals r.Id
                             select r.Name!).ToList()
                })
                .ToListAsync(cancellationToken);
        }
    }
}
