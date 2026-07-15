using GymAppApi.Domain.Models;

namespace GymAppApi.Services.Token
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user, IList<string> roles);
        RefreshToken GenerateRefreshToken(Guid userId);
    }
}
