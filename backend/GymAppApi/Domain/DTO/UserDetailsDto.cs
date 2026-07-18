namespace GymAppApi.Domain.DTO
{
    public class UserDetailsDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime Birthday { get; set; } = DateTime.MinValue;
        public List<string> Roles { get; set; } = new List<string>();
    }
}
