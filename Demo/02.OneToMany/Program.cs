namespace _02.OneToMany
{
    using Data;

    class Program
    {
        static void Main(string[] args)
        {
            EmplDbContext db = new EmplDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            //Department dep = new Department { Name = "Test" };
            //db.Employees.Add(new Employee { Name = "Pesho"});

            //db.Departments.Add(dep);
            db.SaveChanges();
        }
    }
}
