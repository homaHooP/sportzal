using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Users.Commands
{
    public class GiveUserRoleCommand : IRequest<UserDetailsDto>
    {
        public Guid UserId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
