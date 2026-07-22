using GymAppApi.Application.Common;
using GymAppApi.Domain.DTO;
using MediatR;

namespace GymAppApi.Application.Memberships.Queries
{
    public class GetClientMembershipsCommand : IRequest<ClientMembershipsDto>, IClientOwnedRequest
    {
        public Guid ClientId { get; set; }
        public Guid GetOwnerId() => ClientId;
    }
}