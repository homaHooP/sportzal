using MediatR;

namespace GymAppApi.Application.Users.Commands
{
    public class LogoutCommand : IRequest
    {
        public string Token { get; set; } = string.Empty;
    }
}
