using MediatR;

namespace GymAppApi.Application.Gyms.Commands
{
    public class DeleteGymManagerCommand : IRequest
    {
        public Guid GymId { get; set; }
        public Guid ManagerId { get; set; }
    }
}
