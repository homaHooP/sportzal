using GymAppApi.Application.Common;
using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Users.Queries
{
    public class GetUsersCommand : IRequest<IEnumerable<UserDto>>
    { 
    }
}
