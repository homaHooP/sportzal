using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Users.Commands.Promotions
{
    public class PromoteClientToTrainerCommand : IRequest<UserDetailsDto>
    {
        public Guid ClientId { get; set; }
        public string Specialty { get; set; } = string.Empty;
    }
}
