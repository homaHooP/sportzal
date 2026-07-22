using FluentValidation.Results;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Data;
using GymAppApi.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GymAppApi.Domain.Models;

namespace GymAppApi.Application.Gyms.Commands
{
    public class DeleteGymHandler(GymAppDbContext _context,UserManager<User> _userManager,GymManagerDemotionService _ds) : IRequestHandler<DeleteGymCommand>
    {
        public async Task Handle(DeleteGymCommand command, CancellationToken ct)
        {
            var gym = await _context.Gyms.FirstOrDefaultAsync(x => x.Id == command.GymId, ct);
            if (gym == null) { throw new NotFoundException("Gym", command.GymId); }
            if (gym.DeletedAt != null) {
                throw new ValidationException(new List<ValidationFailure>
                { new ValidationFailure("Gym", "Gym is already deleted") });
            }

            var hasFutureSessions = await _context.Sessions.AnyAsync(s => s.GymId == gym.Id && s.StartTime > DateTime.UtcNow, ct);
            if (hasFutureSessions)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new ValidationFailure("Gym", "Cannot delete a gym with upcoming sessions") });
            }

            var managers = await _context.UserGymManagers.Where(m => m.GymId == gym.Id).ToListAsync(ct);
            foreach (var manager in managers)
            {
                var managerUser = await _userManager.FindByIdAsync(manager.UserId.ToString());
                await _ds.DemoteAsync(managerUser!, ct);
            }
            _context.UserGymManagers.RemoveRange(managers);

            gym.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(ct);
        }
    }
}
