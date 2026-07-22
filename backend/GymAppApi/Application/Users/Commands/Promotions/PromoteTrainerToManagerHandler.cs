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
    public class PromoteTrainerToManagerHandler(CurrentUserService cus, UserManager<User> _userManager, GymAppDbContext _context, ISender _sender) : IRequestHandler<PromoteTrainerToManagerCommand,UserDetailsDto>
    {
        public async Task<UserDetailsDto> Handle(PromoteTrainerToManagerCommand command, CancellationToken ct)
        {
            if (cus.UserId == command.TrainerId)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("Current user", "You cant promote yourself")
                });
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == command.TrainerId, ct);
            if (user == null)
            {
                throw new NotFoundException("User", command.TrainerId);
            }
            if (user.WasDeactivated != null)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("User deactivated", "User is deactivated")
                });
            }
            var gym = await _context.Gyms.FirstOrDefaultAsync(x => x.Id == command.GymId, ct);
            if (gym == null)
            {
                throw new NotFoundException("Gym", command.GymId);
            }

            if (await _userManager.IsInRoleAsync(user, RoleNames.Manager))
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("User role", "User is already a manager")
                });
            }
            var isInTrainers = await _context.UserTrainers.FirstOrDefaultAsync(x => x.UserId == user.Id, ct);
            if (!await _userManager.IsInRoleAsync(user, RoleNames.Trainer) || isInTrainers == null)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("User role", "User isn't a trainer")
                });
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(ct);

            await _userManager.RemoveFromRoleAsync(user, RoleNames.Trainer);
            await _userManager.AddToRoleAsync(user, RoleNames.Manager);

            var manager = await _context.UserGymManagers.FirstOrDefaultAsync(t => t.UserId == user.Id, ct);
            if (manager is null)
            {
                manager = new UserGymManager { UserId = user.Id, GymId = gym.Id };
                _context.UserGymManagers.Add(manager);
            }

            var oldTrainer = await _context.UserTrainers
                .Include(c => c.Sessions)
                .FirstOrDefaultAsync(c => c.UserId == user.Id, ct);
            if (oldTrainer is not null)
            {
                if (!oldTrainer.Sessions.Any())
                {
                    _context.UserTrainers.Remove(oldTrainer);
                }
            }

            await _context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return await _sender.Send(new GetUserByIdCommand { Id = command.TrainerId }, ct);
        }
    }
}
