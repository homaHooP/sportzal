using GymAppApi.Domain.Enums;

namespace GymAppApi.Domain.DTO
{
    public class SessionDto
    {
        public Guid Id { get; set; }
        public Guid GymId { get; set; }
        public string GymName { get; set; } = string.Empty;
        public Guid TrainerId { get; set; }
        public string TrainerName { get; set; } = string.Empty;
        public SessionType Type { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MaxParticipants { get; set; }
        public DateTime? CancelledAt { get; set; }
        public bool IsAvailable { get; set; }
    }
}
