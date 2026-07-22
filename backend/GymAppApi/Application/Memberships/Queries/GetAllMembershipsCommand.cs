using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Memberships.Queries
{
    public class GetAllMembershipsCommand : IRequest<List<MembershipDto>>
    {
    }
}
