namespace GameStore.Data
{
    using Entities;
    using Microsoft.EntityFrameworkCore;

    public class GameStoreMvcDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        //public DbSet<Game> Games { get; set; }

        //public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=.;Database=GameStoreMvc;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            //builder.Entity<User>()
            //    .HasMany(u => u.Games)
            //    .WithOne(g => g.User)
            //    .HasForeignKey(g => g.UserId);

            //builder.Entity<Game>()
            //    .HasMany(g => g.Users)
            //    .WithOne(u => u.Game)
            //    .HasForeignKey(u => u.GameId);
        }
    }
}
