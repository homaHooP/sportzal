using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GymAppApi.Domain.Enums;

namespace GymAppApi.Domain.Models
{
    public class Session
    {

        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Session type is required (individual/group)")]
        public SessionType Type { get; set; }

        [Required]
        public Guid TrainerId { get; set; }

        [ForeignKey(nameof(TrainerId))]
        public UserTrainer Trainer { get; set; } = null!;

        [Required]
        public Guid GymId { get; set; }

        [ForeignKey(nameof(GymId))]
        public Gym Gym { get; set; } = null!;

        [Required(ErrorMessage = "Start time is required")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Max participants value is required")]
        [Range(1, 100, ErrorMessage = "Max participants must be between 1 and 100")]
        public int MaxParticipants { get; set; } = 1;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
