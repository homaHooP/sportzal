using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Users.Commands
{
    public class RegisterCommand : IRequest<AuthResultDto>
    {
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}
