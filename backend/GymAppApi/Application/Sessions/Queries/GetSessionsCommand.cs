using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Sessions.Queries
{
    public class GetSessionsCommand : IRequest<List<SessionDto>>
    {

    }
}
