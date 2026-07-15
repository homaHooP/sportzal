using GymAppApi.Domain.DTO;
using MediatR;
using System.Windows.Input;

namespace GymAppApi.Application.Users.Commands
{
    public class LoginCommand : IRequest<AuthResultDto>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
