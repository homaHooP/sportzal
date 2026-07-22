using GymAppApi.Data;
using GymAppApi.Domain.Constants;
using GymAppApi.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Services
{
    public class GymManagerDemotionService(UserManager<User> _userManager, GymAppDbContext _context)
    {
        public async Task DemoteAsync(User user, CancellationToken ct)
        {
            await _userManager.RemoveFromRoleAsync(user, RoleNames.Manager);
            await _userManager.AddToRoleAsync(user, RoleNames.Client);

            var oldManager = await _context.UserGymManagers
                .FirstOrDefaultAsync(m => m.UserId == user.Id, ct);

            if (oldManager is not null)
            {
                _context.UserGymManagers.Remove(oldManager);
            }
            var newClient = await _context.UserClients
                .FirstOrDefaultAsync(c => c.UserId == user.Id, ct);
            if (newClient == null)
            {
                newClient = new UserClient { UserId = user.Id };
                _context.UserClients.Add(newClient);
            }
        }
    }
}
