using MediatR;

namespace GymAppApi.Application.Users.Commands
{
    public class DeleteUserRoleCommand : IRequest
    {
        public Guid UserId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
