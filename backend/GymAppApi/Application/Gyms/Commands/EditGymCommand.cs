using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Gyms.Commands
{
    public class EditGymCommand : IRequest<GymDetailsDto>
    {
        public Guid GymId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
