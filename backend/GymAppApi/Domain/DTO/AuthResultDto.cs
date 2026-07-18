using GymAppApi.Domain.Models;

namespace GymAppApi.Domain.DTO
{
    public class AuthResultDto
    {
        public AuthResultDto(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; }
    }
}
