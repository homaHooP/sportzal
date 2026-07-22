using GymAppApi.Domain.DTO;
using GymAppApi.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using GymAppApi.Domain.Constants;
using FluentValidation.Results;
using GymAppApi.Data;
using GymAppApi.Application.Users.Queries;

namespace GymAppApi.Application.Users.Commands.Promotions
{
    public class PromoteClientToTrainerHandler(GymAppDbContext _context, UserManager<User> _userManager, ISender _sender, CurrentUserService cus) : IRequestHandler<PromoteClientToTrainerCommand,UserDetailsDto>
    {
        public async Task<UserDetailsDto> Handle(PromoteClientToTrainerCommand command, CancellationToken ct)
        {
            if (cus.UserId == command.ClientId)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("Current user", "You cant promote yourself")
                });
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x=>x.Id == command.ClientId, ct);
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

            if (await _userManager.IsInRoleAsync(user,RoleNames.Trainer))
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("User role", "User is already a trainer")
                });
            }

            var isInClients = await _context.UserClients.FirstOrDefaultAsync(x => x.UserId == user.Id,ct);

            if (!await _userManager.IsInRoleAsync(user, RoleNames.Client) || isInClients == null)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("User role", "User isn't a client")
                });
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(ct);

            await _userManager.RemoveFromRoleAsync(user, RoleNames.Client);
            await _userManager.AddToRoleAsync(user, RoleNames.Trainer);

            var trainer = await _context.UserTrainers.FirstOrDefaultAsync(t => t.UserId == user.Id, ct);
            if (trainer is null)
            {
                trainer = new UserTrainer { UserId = user.Id };
                _context.UserTrainers.Add(trainer);
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
