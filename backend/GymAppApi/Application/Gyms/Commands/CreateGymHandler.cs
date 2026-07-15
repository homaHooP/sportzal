using GymAppApi.Data;
using GymAppApi.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Gyms.Commands
{
    public class CreateGymHandler(GymAppDbContext _context) : IRequestHandler<CreateGymCommand, Gym>
    {
        public async Task<Gym> Handle(CreateGymCommand request, CancellationToken cancellationToken)
        {
            var gym = new Gym
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Address = request.Address
            };

            _context.Gyms.Add(gym);
            await _context.SaveChangesAsync(cancellationToken);

            return gym;
        }
    }
}
