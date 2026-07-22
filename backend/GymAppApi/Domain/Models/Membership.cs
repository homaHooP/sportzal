using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GymAppApi.Domain.Enums;

namespace GymAppApi.Domain.Models
{
    public class Membership
    {
        [Key] 
        public Guid Id { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public UserClient Client { get; set; } = null!;

        [Required]
        public Guid GymId { get; set; }

        [ForeignKey(nameof(GymId))]
        public Gym Gym { get; set; }

        [Required(ErrorMessage = "Membership type is required")]
        public MembershipType Type { get; set; }

        [Required(ErrorMessage = "Membership start date is required")]
        public DateTime StartDate { get; set; } = DateTime.MinValue;

        [Required(ErrorMessage = "Membership end date is required")]
        public DateTime EndDate { get; set; } = DateTime.MinValue;
        public DateTime? NullifiedAt { get; set; }
        public bool IsActive => NullifiedAt == null && EndDate > DateTime.UtcNow;

        [Required(ErrorMessage = "Sessions left value is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Sessions left must be a non-negative value")]
        public int SessionsLeft { get; set; } = 0;
    }
}
