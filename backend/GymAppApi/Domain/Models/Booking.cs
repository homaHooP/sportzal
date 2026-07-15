using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GymAppApi.Domain.Enums;

namespace GymAppApi.Domain.Models
{
    public class Booking
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public UserClient Client { get; set; } = null!;

        [Required]
        public Guid SessionId { get; set; }

        [ForeignKey(nameof(SessionId))]
        public Session Session { get; set; } = null!;

        [Required]
        public BookingStatus Status { get; set; } = BookingStatus.Booked;

        public DateTime BookedAt { get; set; } = DateTime.UtcNow;
    }
}