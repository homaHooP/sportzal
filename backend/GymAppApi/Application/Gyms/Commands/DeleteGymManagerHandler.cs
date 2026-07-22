using FluentValidation.Results;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GymAppApi.Domain.Models;
using GymAppApi.Domain.Constants;
using GymAppApi.Services;

namespace GymAppApi.Application.Gyms.Commands
{
    public class DeleteGymManagerHandler(GymAppDbContext _context, UserManager<User> _userManager, GymManagerDemotionService _ds) : IRequestHandler<DeleteGymManagerCommand>
    {
        public async Task Handle(DeleteGymManagerCommand command, CancellationToken ct)
        {
            var gym = await _context.Gyms.FirstOrDefaultAsync(x => x.Id == command.GymId, ct);
            if (gym == null) { throw new NotFoundException("Gym", command.GymId); }
            if (gym.DeletedAt != null)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new ValidationFailure("Gym", "Gym is deleted") });
            }

            var manager = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.ManagerId, ct);
            if (manager == null) { throw new NotFoundException("Manager", command.ManagerId); }
            //Якщо менеджер видалений на рівні user, а він залишився менеджером в залі, headManager зможе видалити фантомний аканут 
            //if (manager.WasDeactivated != null)
            //{
            //    throw new ValidationException(new List<ValidationFailure>
            //    { new ValidationFailure("Manager", "User is deleted") });
            //}
            var m = await _context.UserGymManagers.FirstOrDefaultAsync(x => x.UserId == manager.Id && x.GymId == gym.Id, ct);
            if (m == null || !await _userManager.IsInRoleAsync(manager, RoleNames.Manager) || !gym.Managers.Contains(m))
            {
                throw new ValidationException(new List<ValidationFailure>
                { new ValidationFailure("Manager", "User isn't a manager in this gym") });
            }

            //Повне видалення менеджера
            await _ds.DemoteAsync(manager, ct);
            _context.UserGymManagers.Remove(m);
            await _context.SaveChangesAsync(ct);
        }
    }
}
