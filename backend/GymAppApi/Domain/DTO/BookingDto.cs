using GymAppApi.Domain.Enums;

namespace GymAppApi.Domain.DTO
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid SessionId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string TrainerName { get; set; } = string.Empty;
        public DateTime SessionStartTime { get; set; }
        public DateTime SessionEndTime { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime BookedAt { get; set; }
    }
}