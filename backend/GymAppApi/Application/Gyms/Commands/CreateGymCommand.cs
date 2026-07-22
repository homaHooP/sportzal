using MediatR;
using GymAppApi.Domain.Models;
using GymAppApi.Domain.DTO;

namespace GymAppApi.Application.Gyms.Commands
{
    public class CreateGymCommand : IRequest<GymDetailsDto>
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
