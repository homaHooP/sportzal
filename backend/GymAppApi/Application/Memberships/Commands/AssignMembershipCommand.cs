using GymAppApi.Application.Common;
using GymAppApi.Domain.DTO;
using GymAppApi.Domain.Enums;
using MediatR;

namespace GymAppApi.Application.Memberships.Commands
{
    public class AssignMembershipCommand : IRequest<MembershipDto>, IClientOwnedRequest
    {
        public Guid ClientId { get; set; }
        public Guid GymId { get; set; }
        public MembershipType Type { get; set; }
        public DateTime EndDate { get; set; }
        public int SessionsLeft { get; set; }

        public Guid GetOwnerId() => ClientId;
    }
}