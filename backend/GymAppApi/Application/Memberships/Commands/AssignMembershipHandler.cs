using FluentValidation.Results;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Data;
using GymAppApi.Domain.Constants;
using GymAppApi.Domain.DTO;
using GymAppApi.Domain.Models;
using GymAppApi.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Memberships.Commands
{
    public class AssignMembershipHandler(GymAppDbContext _context, CurrentUserService _cus)
        : IRequestHandler<AssignMembershipCommand, MembershipDto>
    {
        public async Task<MembershipDto> Handle(AssignMembershipCommand command, CancellationToken ct)
        {
            var client = await _context.UserClients.Include(c=>c.User).FirstOrDefaultAsync(c => c.UserId == command.ClientId, ct);
            if (client == null) { throw new NotFoundException("Client", command.ClientId); }
            if (client.User.WasDeactivated != null) {
                throw new ValidationException(new List<ValidationFailure>
                { new("User", "Cant assign to a deactivated user") });
            }

            var gym = await _context.Gyms.FirstOrDefaultAsync(g => g.Id == command.GymId, ct);
            if (gym == null) { throw new NotFoundException("Gym", command.GymId); }
            if (gym.DeletedAt != null)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Gym", "Gym is deleted") });
            }

            var isSelf = _cus.UserId == command.ClientId;
            if (!isSelf && !_cus.IsInRole(RoleNames.HeadManager))
            {
                var managesThisGym = await _context.UserGymManagers
                    .AnyAsync(m => m.UserId == _cus.UserId && m.GymId == command.GymId, ct);

                if (!managesThisGym)
                {
                    throw new ForbiddenAccessException("You don't manage this gym");
                }
            }

            var hasActive = await _context.Memberships.AnyAsync(m =>
                m.ClientId == command.ClientId &&
                m.NullifiedAt == null &&
                m.EndDate > DateTime.UtcNow, ct);

            if (hasActive)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Membership", "Client already has an active membership. Nullify it first.") });
            }

            var membership = new Membership
            {
                ClientId = command.ClientId,
                GymId = command.GymId,
                Type = command.Type,
                StartDate = DateTime.UtcNow,
                EndDate = command.EndDate,
                SessionsLeft = command.SessionsLeft
            };

            _context.Memberships.Add(membership);
            await _context.SaveChangesAsync(ct);

            return new MembershipDto
            {
                Id = membership.Id,
                GymId = gym.Id,
                GymName = gym.Name,
                Type = membership.Type,
                StartDate = membership.StartDate,
                EndDate = membership.EndDate,
                SessionsLeft = membership.SessionsLeft,
                NullifiedAt = null,
                IsActive = true
            };
        }
    }
}