using FluentValidation.Results;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Data;
using GymAppApi.Domain.Constants;
using GymAppApi.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Memberships.Commands
{
    public class NullifyMembershipHandler(GymAppDbContext _context, CurrentUserService _cus): IRequestHandler<NullifyMembershipCommand>
    {
        public async Task Handle(NullifyMembershipCommand command, CancellationToken ct)
        {
            var membership = await _context.Memberships
                .FirstOrDefaultAsync(m => m.Id == command.MembershipId, ct);

            if (membership == null) { throw new NotFoundException("Membership", command.MembershipId); }

            var isSelf = _cus.UserId == membership.ClientId;

            if (!isSelf && !_cus.IsInRole(RoleNames.HeadManager))
            {
                if (_cus.IsInRole(RoleNames.Manager))
                {
                    var managesThisGym = await _context.UserGymManagers
                        .AnyAsync(m => m.UserId == _cus.UserId && m.GymId == membership.GymId, ct);

                    if (!managesThisGym)
                    {
                        throw new ForbiddenAccessException("You don't manage the gym this membership belongs to");
                    }
                }
                else
                {
                    throw new ForbiddenAccessException("You can only nullify your own membership");
                }
            }

            if (membership.NullifiedAt != null)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Membership", "Membership is already nullified") });
            }

            if (membership.EndDate <= DateTime.UtcNow)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new("Membership", "Membership has already expired") });
            }

            membership.NullifiedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(ct);
        }
    }
}