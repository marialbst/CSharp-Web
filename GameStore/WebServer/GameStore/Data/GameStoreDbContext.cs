namespace WebServer.GameStore.Data
{ 
    using Microsoft.EntityFrameworkCore;

    public class GameStoreDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=.;Database=GameStoreDb;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
    }
}
