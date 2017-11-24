namespace WebServer.ByTheCake.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class CakeList
    {
        private const string DbPath = "./ByTheCake/Resources/Data/database.csv";

        private IEnumerable<Cake> cakes;

        public CakeList()
        {
            this.cakes = new List<Cake>();
        }
     
        public void Add(string name, string price)
        {
            var id = 1;

            if (File.Exists(DbPath))
            {
                var streamReader = new StreamReader(DbPath);
                id = streamReader.ReadToEnd().Split(Environment.NewLine).Length;
                streamReader.Dispose();
            }
            else
            {
                var file = File.Create(DbPath);
                file.Close();
            }

            using (var streamWriter = new StreamWriter(DbPath, true))
            {
                streamWriter.WriteLine($"{id},{name},{price}");
            }
        }

        public IEnumerable<Cake> AllCakes()
        {
            return File
                .ReadAllLines(DbPath)
                .Where(l => l.Contains(','))
                .Select(l => l.Split(','))
                .Select(l => new Cake
                {
                    Id = int.Parse(l[0]),
                    Name = l[1],
                    Price = decimal.Parse(l[2])
                });
        }
        
        public Cake GetCakeById(int id)
        {
            return this.AllCakes().FirstOrDefault(c => c.Id == id);
        }
    }
}
