using GymAppApi.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserTrainer
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [Required(ErrorMessage = "Trainer specialty is required")]
    [MaxLength(30)]
    [MinLength(4)]
    public string Specialty { get; set; } = string.Empty;
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}