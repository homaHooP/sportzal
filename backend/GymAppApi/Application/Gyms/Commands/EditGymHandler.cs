using FluentValidation.Results;
using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Application.Gyms.Queries;
using GymAppApi.Data;
using GymAppApi.Domain.Constants;
using GymAppApi.Domain.DTO;
using GymAppApi.Domain.Models;
using GymAppApi.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Gyms.Commands
{
    public class EditGymHandler(GymAppDbContext _context, CurrentUserService cus) : IRequestHandler<EditGymCommand,GymDetailsDto>
    {
        public async Task<GymDetailsDto> Handle(EditGymCommand command, CancellationToken ct)
        {
            var gym = await _context.Gyms.FirstOrDefaultAsync(x => x.Id == command.GymId, ct);
            if (gym == null) { throw new NotFoundException("Gym", command.GymId); }
            if (gym.DeletedAt != null)
            {
                throw new ValidationException(new List<ValidationFailure>
                { new ValidationFailure("Gym", "Cannot edit a deleted gym") });
            }
            var isManagerOfThisGym = await _context.UserGymManagers
                .AnyAsync(m => m.UserId == cus.UserId && m.GymId == command.GymId, ct);

            if (!isManagerOfThisGym && !cus.IsInRole(RoleNames.HeadManager))
            {
                throw new ForbiddenAccessException();
            }

            gym.Name = command.Name;
            gym.Address = command.Address;

            await _context.SaveChangesAsync(ct);

            GymDetailsDto dto = new GymDetailsDto
            {
                Id = command.GymId,
                Name = gym.Name,
                Address = gym.Address,
                ManagerNames = gym.Managers.Select(x => x.User.Email).ToList(),
                SessionsCount = gym.Sessions.Count,
                MembershipsCount = gym.Memberships.Count
            };

            return dto;
        }
    }
}
