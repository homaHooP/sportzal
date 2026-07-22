using MediatR;

namespace GymAppApi.Application.Sessions.Commands
{
    public class CancelSessionCommand : IRequest
    {
        public Guid SessionId { get; set; }
    }
}