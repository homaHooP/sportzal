using FluentValidation.Results;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Domain.Models;
using GymAppApi.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GymAppApi.Domain.Constants;

namespace GymAppApi.Application.Users.Commands
{
    public class DeleteUserHandler(UserManager<User> _userManager, CurrentUserService cus) : IRequestHandler<DeleteUserCommand>
    {
        public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == command.UserId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User", command.UserId);
            }
            if (cus.UserId == command.UserId)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("Friendly fire", "You can't delete yourself :)")
                });
            }
            if (cus.IsInRole(RoleNames.Manager) && await _userManager.IsInRoleAsync(user, RoleNames.HeadManager))
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("No rights", "You can't delete this user")
                });
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("Internal error", "Error occured while deleting the user: " + string.Join(", ", result.Errors.Select(e => e.Description)))
                });
            }
        }
    }
}
