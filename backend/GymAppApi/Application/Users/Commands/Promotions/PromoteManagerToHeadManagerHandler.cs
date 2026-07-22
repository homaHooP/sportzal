using FluentValidation.Results;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Application.Users.Queries;
using GymAppApi.Data;
using GymAppApi.Domain.Constants;
using GymAppApi.Domain.DTO;
using GymAppApi.Domain.Models;
using GymAppApi.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Users.Commands.Promotions
{
    public class PromoteManagerToHeadManagerHandler(CurrentUserService cus, UserManager<User> _userManager, GymAppDbContext _context, ISender _sender) : IRequestHandler<PromoteManagerToHeadManagerCommand,UserDetailsDto>
    {
        public async Task<UserDetailsDto> Handle(PromoteManagerToHeadManagerCommand command, CancellationToken ct)
        {
            if (cus.UserId == command.ManagerId)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("Current user", "You cant promote yourself")
                });
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == command.ManagerId, ct);
            if (user == null)
            {
                throw new NotFoundException("User", command.ManagerId);
            }
            if (user.WasDeactivated != null)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("User deactivated", "User is deactivated")
                });
            }

            if (await _userManager.IsInRoleAsync(user, RoleNames.HeadManager))
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("User role", "User is already a head manager")
                });
            }

            var isInManagers = await _context.UserGymManagers.FirstOrDefaultAsync(x => x.UserId == user.Id, ct);

            if (!await _userManager.IsInRoleAsync(user, RoleNames.Manager) || isInManagers == null)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("User role", "User isn't a manager")
                });
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(ct);

            await _userManager.RemoveFromRoleAsync(user, RoleNames.Manager);
            await _userManager.AddToRoleAsync(user, RoleNames.HeadManager);

            var oldManager = await _context.UserGymManagers
                .Include(c => c.ManagedGym)
                .FirstOrDefaultAsync(c => c.UserId == user.Id, ct);
            if (oldManager is not null)
            {
                _context.UserGymManagers.Remove(oldManager);
            }

            await _context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return await _sender.Send(new GetUserByIdCommand { Id = command.ManagerId }, ct);
        }
    }
}
