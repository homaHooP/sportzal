using MediatR;
using GymAppApi.Domain.DTO;

namespace GymAppApi.Application.Users.Commands.Outdated
{
    public class DeleteUserRoleCommand : IRequest
    {
        public Guid UserId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
