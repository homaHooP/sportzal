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
using System.Runtime.CompilerServices;

namespace GymAppApi.Application.Users.Commands.Promotions
{
    public class PromoteClientToManagerHandler(ISender _sender, GymAppDbContext _context, UserManager<User> _userManager, CurrentUserService cus) : IRequestHandler<PromoteClientToManagerCommand,UserDetailsDto>
    {
        public async Task<UserDetailsDto> Handle (PromoteClientToManagerCommand command, CancellationToken ct)
        {
            if (cus.UserId == command.ClientId)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("Current user", "You cant promote yourself")
                });
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == command.ClientId, ct);
            if (user == null)
            {
                throw new NotFoundException("User", command.ClientId);
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
            var isInClients = await _context.UserClients.FirstOrDefaultAsync(x => x.UserId == user.Id, ct);
            if (!await _userManager.IsInRoleAsync(user, RoleNames.Client) || isInClients == null)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("User role", "User isn't a client")
                });
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(ct);

            await _userManager.RemoveFromRoleAsync(user, RoleNames.Client);
            await _userManager.AddToRoleAsync(user, RoleNames.Manager);

            var manager = await _context.UserGymManagers.FirstOrDefaultAsync(t => t.UserId == user.Id, ct);
            if (manager is null)
            {
                manager = new UserGymManager { UserId = user.Id , GymId = gym.Id};
                _context.UserGymManagers.Add(manager);
            }

            var oldClient = await _context.UserClients
                .Include(c => c.Bookings)
                .Include(c => c.Memberships)
                .FirstOrDefaultAsync(c => c.UserId == user.Id, ct);
            if (oldClient is not null)
            {
                if (!oldClient.Bookings.Any() && !oldClient.Memberships.Any())
                {
                    _context.UserClients.Remove(oldClient);
                }
            }

            await _context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return await _sender.Send(new GetUserByIdCommand { Id = command.ClientId }, ct);
        }
    }
}
