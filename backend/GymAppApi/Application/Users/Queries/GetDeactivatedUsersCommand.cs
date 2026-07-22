using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Users.Queries
{
    public class GetDeactivatedUsersCommand : IRequest<IEnumerable<UserDto>>
    {
    }
}
