using GymAppApi.Domain.Enums;

namespace GymAppApi.Domain.DTO
{
    public class MembershipDto
    {
        public Guid Id { get; set; }
        public Guid GymId { get; set; }
        public string GymName { get; set; } = string.Empty;
        public MembershipType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SessionsLeft { get; set; }
        public DateTime? NullifiedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
