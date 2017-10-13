using Microsoft.EntityFrameworkCore;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new CustomDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.People.Add(new Person { Name = "Pesho", Age = 15 });
            db.SaveChanges();
        }
    }
}
