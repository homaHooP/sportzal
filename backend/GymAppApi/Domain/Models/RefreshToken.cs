using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymAppApi.Domain.Models
{
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime? RevokedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string? ReplacedByToken { get; set; }
        public bool IsActive => RevokedAt == null && ExpiresAt > DateTime.UtcNow;
    }
}
