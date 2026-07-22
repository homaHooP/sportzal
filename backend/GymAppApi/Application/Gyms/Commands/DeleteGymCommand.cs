using MediatR;

namespace GymAppApi.Application.Gyms.Commands
{
    public class DeleteGymCommand : IRequest
    {
        public Guid GymId { get; set; }
    }
}