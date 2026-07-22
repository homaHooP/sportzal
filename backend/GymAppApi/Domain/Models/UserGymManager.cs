using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymAppApi.Domain.Models
{
    public class UserGymManager
    {
        [Key]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [Required]
        public Guid GymId { get; set; }

        [ForeignKey(nameof(GymId))]
        public Gym ManagedGym { get; set; } = null!;
    }
}
