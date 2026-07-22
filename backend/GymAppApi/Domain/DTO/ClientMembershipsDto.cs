namespace GymAppApi.Domain.DTO
{
    public class ClientMembershipsDto
    {
        public MembershipDto? Active { get; set; }
        public List<MembershipDto> History { get; set; } = new();
    }
}
