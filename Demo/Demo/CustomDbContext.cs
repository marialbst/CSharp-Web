using Microsoft.EntityFrameworkCore;

namespace Demo
{
    public class CustomDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=.;Database=CoreTestDb;Integrated Security=True");

            base.OnConfiguring(builder);
        }
    }
}
