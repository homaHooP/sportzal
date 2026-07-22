namespace GymAppApi.Domain.DTO
{
    public class GymDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<string> ManagerNames { get; set; } = new List<string>();
        public string DeletedAt { get; set; } = string.Empty;
        public int SessionsCount { get; set; }
        public int MembershipsCount { get; set; }
    }
}
