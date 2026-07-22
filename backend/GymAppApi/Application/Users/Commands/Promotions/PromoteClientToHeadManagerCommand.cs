using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Users.Commands.Promotions
{
    public class PromoteClientToHeadManagerCommand : IRequest<UserDetailsDto>
    {
        public Guid ClientId { get; set; }
    }
}
