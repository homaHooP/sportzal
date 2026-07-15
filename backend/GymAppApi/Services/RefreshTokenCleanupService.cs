using GymAppApi.Data;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Services
{
    public class RefreshTokenCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromHours(24);

        public RefreshTokenCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<GymAppDbContext>();

                    var cutoff = DateTime.UtcNow.AddDays(-30);
                    var staleTokens = await db.RefreshTokens
                        .Where(rt => rt.ExpiresAt < cutoff || (rt.RevokedAt != null && rt.RevokedAt < cutoff))
                        .ToListAsync(stoppingToken);

                    if (staleTokens.Count > 0)
                    {
                        db.RefreshTokens.RemoveRange(staleTokens);
                        await db.SaveChangesAsync(stoppingToken);
                    }
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
