using GymAppApi.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Application.Users.Commands
{
    public class LogoutHandler(GymAppDbContext _db) : IRequestHandler<LogoutCommand>
    {
        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var token = await _db.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == request.Token, cancellationToken);

            if (token != null && token.RevokedAt == null)
            {
                token.RevokedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync(cancellationToken);
            }
        }
    }
}