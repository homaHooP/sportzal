using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Users.Commands.Promotions
{
    public class PromoteManagerToHeadManagerCommand : IRequest<UserDetailsDto>
    {
        public Guid ManagerId { get; set; }
    }
}
