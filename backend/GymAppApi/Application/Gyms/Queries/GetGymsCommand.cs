using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Gyms.Queries
{
    public class GetGymsCommand : IRequest<List<GymDto>>
    {
    }
}
