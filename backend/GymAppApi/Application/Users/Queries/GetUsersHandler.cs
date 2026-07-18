using GymAppApi.Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using GymAppApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using GymAppApi.Services;
using GymAppApi.Data;

namespace GymAppApi.Application.Users.Queries
{
    public class GetUsersHandler(GymAppDbContext _context) : IRequestHandler<GetUsersCommand,IEnumerable<UserDto>>
    {
        public async Task<IEnumerable<UserDto>> Handle(GetUsersCommand command, CancellationToken cancellationToken)
        {
            return await _context.Users
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
