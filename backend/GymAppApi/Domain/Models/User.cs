using Microsoft.AspNetCore.Identity;

namespace GymAppApi.Domain.Models
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime Birthday { get; set; } = DateTime.MinValue;
    }
}