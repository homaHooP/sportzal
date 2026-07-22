using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Users.Commands.Promotions
{
    public class PromoteTrainerToManagerCommand : IRequest<UserDetailsDto>
    {
        public Guid TrainerId { get; set; }
        public Guid GymId { get; set; }
    }
}
