using DatabaseFirst.Models;

namespace DatabaseFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            BlogDbContext db = new BlogDbContext();

            using (db)
            {

            }
        }
    }
}
