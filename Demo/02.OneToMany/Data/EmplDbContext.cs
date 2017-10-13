namespace _02.OneToMany.Data
{
    using Models;
    using Microsoft.EntityFrameworkCore;

    public class EmplDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=.;Database=OneToManyCoreDb;Integrated Security=True");

            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Employee>()
                .HasOne(emp => emp.Department)
                .WithMany(dep => dep.Employees)
                .HasForeignKey(emp => emp.DepartmentId);

            builder.Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany(m => m.Slaves)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
