using FluentValidation.Results;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Domain.Models;
using GymAppApi.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GymAppApi.Domain.Constants;
using GymAppApi.Data;

namespace GymAppApi.Application.Users.Commands
{
    public class DeleteUserHandler(GymAppDbContext _context, UserManager<User> _userManager, CurrentUserService cus) : IRequestHandler<DeleteUserCommand>
    {
        public async Task Handle(DeleteUserCommand command, CancellationToken ct)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == command.UserId, ct);
            if (user == null)
                throw new NotFoundException("User", command.UserId);

            if (cus.UserId == command.UserId)
                throw new ValidationException(new List<ValidationFailure>
                { new ValidationFailure("Friendly fire", "You can't delete yourself :)") });

            if (cus.IsInRole(RoleNames.Manager) && await _userManager.IsInRoleAsync(user, RoleNames.HeadManager))
                throw new ValidationException(new List<ValidationFailure>
                { new ValidationFailure("No rights", "You can't delete this user") });

            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.MaxValue;
            user.WasDeactivated = DateTime.UtcNow;

            var refreshTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == user.Id && rt.RevokedAt != null && rt.ExpiresAt > DateTime.UtcNow)
                .ToListAsync(ct);

            foreach (var token in refreshTokens)
            {
                token.RevokedAt = DateTime.UtcNow;
                token.ExpiresAt = DateTime.UtcNow;
            }

            await _userManager.UpdateAsync(user);
        }
    }
}
