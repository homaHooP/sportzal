using GymAppApi.Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using GymAppApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Services;
using GymAppApi.Data;

namespace GymAppApi.Application.Users.Queries
{
    public class GetUserByIdHandler(GymAppDbContext _context) : IRequestHandler<GetUserByIdCommand,UserDetailsDto>
    {
        public async Task<UserDetailsDto> Handle(GetUserByIdCommand command, CancellationToken cancellationToken)
        {
            var user = await _context.Users
            .Select(u => new UserDetailsDto
            {
                Id = u.Id,
                Email = u.Email ?? string.Empty,
                FullName = u.FullName ?? string.Empty,
                Gender = u.Gender ?? string.Empty,
                Birthday = u.Birthday,
                Roles = (from ur in u.UserRoles
                    join r in _context.Roles on ur.RoleId equals r.Id
                    select r.Name!).ToList()
            }).FirstOrDefaultAsync(u => u.Id == command.Id, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User",command.Id);
            }
            return user;
        }
    }
}
