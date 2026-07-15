using System.ComponentModel.DataAnnotations;

namespace GymAppApi.Domain.Models
{
    public class Gym
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Gym name is required")]
        [MaxLength(50, ErrorMessage = "Gym name can't be longer than 50 chars")]
        [MinLength(4, ErrorMessage = "Gym name can't be shorter than 4 chars")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gym address is required")]
        [RegularExpression("^[а-яА-ЯіІїЇєЄa-zA-Z0-9\\s\\.,\\-\\/№]{5,200}$", ErrorMessage = "Gym address must be between 5 and 200 characters and can only contain letters, numbers and spaces")]
        public string Address { get; set; } = string.Empty;

        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
