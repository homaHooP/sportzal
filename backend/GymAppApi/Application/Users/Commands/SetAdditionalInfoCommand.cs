using GymAppApi.Domain.DTO;
using MediatR;
using GymAppApi.Application.Common;

namespace GymAppApi.Application.Users.Commands
{
    public class SetAdditionalInfoCommand : IRequest<AuthResultDto>, IClientOwnedRequest
    {
        public Guid UserId { get; set; }
        public string Fullname { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime Birthday { get; set; } = DateTime.MinValue;

        public Guid GetOwnerId() => UserId;

    }
}
