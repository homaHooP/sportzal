using GymAppApi.Domain.DTO;
using GymAppApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GymAppApi.Application.Common.Exceptions;
using FluentValidation.Results;
using GymAppApi.Application.Users.Queries;
using GymAppApi.Domain.Constants;
using GymAppApi.Services;

namespace GymAppApi.Application.Users.Commands.Outdated
{
    public class GiveUserRoleHandler(CurrentUserService cus,UserManager<User> _userManager, ISender _sender) : IRequestHandler<GiveUserRoleCommand, UserDetailsDto>
    {
        public async Task<UserDetailsDto> Handle(GiveUserRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User", request.UserId);
            }

            if (await _userManager.IsInRoleAsync(user, request.Role))
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("User role", "User already has this role")
                });
            }

            var currentUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == cus.UserId, cancellationToken);
            if (currentUser == null)
            {
                throw new UnauthorizedAccessException("Current user not found.");
            }
            if (!await _userManager.IsInRoleAsync(currentUser,RoleNames.HeadManager) && request.Role == RoleNames.HeadManager)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("User role", "You don't have the right to give this role")
                });
            }

            var result = await _userManager.AddToRoleAsync(user, request.Role);
            if (!result.Succeeded)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("Internal error", "An error occured while handing user a role: " + string.Join(", ", result.Errors.Select(e => e.Description)))
                });
            }
            return await _sender.Send( new GetUserByIdCommand { Id = request.UserId });
        }
    }
}
