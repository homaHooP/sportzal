using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Memberships.Commands
{
    public class NullifyMembershipCommand : IRequest
    {
        public Guid MembershipId { get; set; }
    }
}