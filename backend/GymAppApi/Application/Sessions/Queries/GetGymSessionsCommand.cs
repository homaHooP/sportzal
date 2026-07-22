using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Sessions.Queries
{
    public class GetGymSessionsCommand : IRequest<List<SessionDto>>
    {
        public Guid GymId { get; set; }
    }
}