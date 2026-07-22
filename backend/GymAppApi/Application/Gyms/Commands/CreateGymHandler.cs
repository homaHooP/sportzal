using GymAppApi.Data;
using GymAppApi.Domain.DTO;
using GymAppApi.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Gyms.Commands
{
    public class CreateGymHandler(GymAppDbContext _context) : IRequestHandler<CreateGymCommand, GymDetailsDto>
    {
        public async Task<GymDetailsDto> Handle(CreateGymCommand request, CancellationToken cancellationToken)
        {
            var gym = new Gym
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Address = request.Address
            };
            _context.Gyms.Add(gym);
            await _context.SaveChangesAsync(cancellationToken);

            var gymDetailsDto = new GymDetailsDto
            {
                Id = gym.Id,
                Name = gym.Name,
                Address = gym.Address
            };

            return gymDetailsDto;
        }
    }
}
