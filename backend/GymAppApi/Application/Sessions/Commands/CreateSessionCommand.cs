using GymAppApi.Domain.DTO;
using GymAppApi.Domain.Enums;
using MediatR;

namespace GymAppApi.Application.Sessions.Commands
{
    public class CreateSessionCommand : IRequest<SessionDto>
    {
        public Guid GymId { get; set; }
        public Guid TrainerId { get; set; }
        public SessionType Type { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MaxParticipants { get; set; } = 1;
    }
}