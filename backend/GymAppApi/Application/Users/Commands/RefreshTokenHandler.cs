using GymAppApi.Application.Common.Exceptions;
using GymAppApi.Data;
using GymAppApi.Domain.DTO;
using GymAppApi.Domain.Models;
using GymAppApi.Services.Token;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Users.Commands
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthResultDto>
    {
        private readonly GymAppDbContext _db;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;

        public RefreshTokenHandler(GymAppDbContext db, ITokenService tokenService, UserManager<User> userManager)
        {
            _db = db;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<AuthResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await _db.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(rt => rt.Token == request.Token, cancellationToken);
            if (token == null) { throw new UnauthorizedAccessException("Invalid refresh token"); }
            if (token.RevokedAt != null)
            {
                var allUserTokens = await _db.RefreshTokens
                    .Where(rt => rt.UserId == token.UserId && rt.RevokedAt == null)
                    .ToListAsync(cancellationToken);

                foreach (var t in allUserTokens)
                    t.RevokedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync(cancellationToken);
                throw new UnauthorizedAccessException("Log in again");
            }
            if (token.ExpiresAt <= DateTime.UtcNow) { throw new UnauthorizedAccessException("Expired refresh token"); }

            var refreshToken = _tokenService.GenerateRefreshToken(token.UserId);
            token.RevokedAt = DateTime.UtcNow;
            token.ReplacedByToken = refreshToken.Token;
            await _db.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            var userRoles = await _userManager.GetRolesAsync(token.User);
            var accessToken = _tokenService.GenerateAccessToken(token.User, userRoles);

            return new AuthResultDto(accessToken, refreshToken.Token);
        }
    }
}
