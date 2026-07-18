using System.Security.Claims;

namespace GymAppApi.Services
{
    public class CurrentUserService(IHttpContextAccessor _httpContextAccessor)
    {
        public Guid UserId
        {
            get
            {
                var idClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return idClaim != null ? Guid.Parse(idClaim) : Guid.Empty;
            }
        }

        public List<string> Roles
        {
            get
            {
                var roleClaim = _httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role)?.Select(c=>c.Value).ToList() ?? new List<string>();
                return roleClaim;
            }
        }

        public bool IsInRole(string role) =>
            _httpContextAccessor.HttpContext?.User.IsInRole(role) ?? false;
    }
}
