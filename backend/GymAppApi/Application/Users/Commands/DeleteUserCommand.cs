using MediatR;

namespace GymAppApi.Application.Users.Commands
{
    public class DeleteUserCommand : IRequest
    {
        public Guid UserId { get; set; }
    }
}
