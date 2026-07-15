using MediatR;
using GymAppApi.Domain.DTO;

namespace GymAppApi.Application.Users.Commands
{
    public class RefreshTokenCommand : IRequest<AuthResultDto>
    {
        public string Token { get; set; } = null!;
    }
}
