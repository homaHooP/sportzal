using GymAppApi.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GymAppApi.Data
{
    public class GymAppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public GymAppDbContext(DbContextOptions<GymAppDbContext> options) : base(options)
        {
        }

        public DbSet<Gym> Gyms { get; set; }
        public DbSet<UserTrainer> UserTrainers { get; set; }
        public DbSet<UserClient> UserClients { get; set; }
        public DbSet<UserGymManager> UserGymManagers { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Gender).IsRequired().HasMaxLength(30);
                entity.Property(u => u.Birthday).IsRequired();
            });

            builder.Entity<Gym>().HasQueryFilter(g => g.DeletedAt == null);

            builder.Entity<User>()
                .HasMany(u => u.UserRoles)
                .WithOne()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<Session>()
                .HasOne(s => s.Trainer)
                .WithMany(t => t.Sessions)
                .HasForeignKey(s => s.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Booking>()
                .HasOne(b => b.Session)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.SessionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Booking>()
                .HasOne(b => b.Client)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Membership>()
                .HasOne(m => m.Client)
                .WithMany(c => c.Memberships)
                .HasForeignKey(m => m.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Membership>()
                .HasOne(m => m.Gym)
                .WithMany(g => g.Memberships)
                .HasForeignKey(m => m.GymId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserClient>().HasKey(uc => uc.UserId);
            builder.Entity<UserTrainer>().HasKey(ut => ut.UserId);
            builder.Entity<UserGymManager>().HasKey(um => um.UserId);

            builder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RefreshToken>()
                .HasIndex(rt => rt.Token)
                .IsUnique();
        }
    }
}
