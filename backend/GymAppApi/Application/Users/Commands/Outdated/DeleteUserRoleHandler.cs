using FluentValidation.Results;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Data;
using GymAppApi.Domain.Constants;
using GymAppApi.Domain.Models;
using GymAppApi.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Users.Commands.Outdated
{
    public class DeleteUserRoleHandler(GymAppDbContext _context, UserManager<User> _userManager, CurrentUserService cus) : IRequestHandler<DeleteUserRoleCommand>
    {
        public async Task Handle(DeleteUserRoleCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == command.UserId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User", command.UserId);
            }

            var roles = await (from ur in _context.UserRoles
                               join r in _context.Roles on ur.RoleId equals r.Id
                               where ur.UserId == command.UserId
                               select r.Name!).ToListAsync(cancellationToken);
            int numOfRoles = roles.Count;
            if (numOfRoles <= 1)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("Roles", "No roles left to delete")
                });
            }

            bool target = roles.Contains(command.Role);
            if  (!target)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("User roles", "User doesn't have this role")
                });
            }
            var currentUser = await _userManager.Users.FirstOrDefaultAsync(x=> x.Id == cus.UserId, cancellationToken);
            if (currentUser == null)
            {
                throw new UnauthorizedAccessException("Current user not found.");
            }
            if (command.Role == RoleNames.HeadManager && !await _userManager.IsInRoleAsync(currentUser, RoleNames.HeadManager))
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("User role", "You don't have the right to delete this role")
                });
            }
            var result = await _userManager.RemoveFromRoleAsync(user, command.Role);
            if (!result.Succeeded)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("Internal error", "Error occured while deleting the user role: " + string.Join(", ", result.Errors.Select(e => e.Description)))
                });
            }
        }
    }
}
