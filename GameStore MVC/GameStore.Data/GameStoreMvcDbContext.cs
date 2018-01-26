namespace GameStore.Data
{
    using Entities;
    using Microsoft.EntityFrameworkCore;

    public class GameStoreMvcDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=.;Database=GameStoreMvc;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>()
                .HasKey(o => new
                {
                    o.UserId,
                    o.GameId
                });

            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<User>()
                .HasMany(u => u.Games)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            builder.Entity<Game>()
                .HasMany(g => g.Users)
                .WithOne(o => o.Game)
                .HasForeignKey(o => o.GameId);
        }
    }
}
