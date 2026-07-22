using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Users.Commands.Promotions
{
    public class PromoteClientToManagerCommand : IRequest<UserDetailsDto>
    {
        public Guid ClientId {  get; set; }
        public Guid GymId { get; set; }
    }
}
