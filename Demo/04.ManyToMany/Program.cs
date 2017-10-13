namespace _04.ManyToMany
{
    using Data;

    class Program
    {
        static void Main(string[] args)
        {
            MyDbContext db = new MyDbContext();
            db.Database.EnsureCreated();
        }
    }
}
