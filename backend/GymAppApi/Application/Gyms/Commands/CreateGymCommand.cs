using MediatR;
using GymAppApi.Domain.Models;

namespace GymAppApi.Application.Gyms.Commands
{
    public class CreateGymCommand : IRequest<Gym>
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
