namespace WebServer.ByTheCake.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class CakeList
    {
        private const string DbPath = "./ByTheCake/Resources/Data/database.csv";

        private static List<Cake> cakes = new List<Cake>();

        public static void SaveCake(Cake cake)
        {
            File.AppendAllText(DbPath, $"{cake.Id},{cake.Name},{cake.Price}{Environment.NewLine}");
        }

        public static List<Cake> SearchCakes(string word)
        {
            List<Cake> result = new List<Cake>();
            string[] cakesFromFile = File.ReadAllText(DbPath).Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in cakesFromFile)
            {
                string[] cakeStr = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (cakeStr.Length != 3)
                {
                    continue;
                }

                if (cakeStr[1].ToLower().Contains(word.ToLower()))
                {
                    result.Add(new Cake(int.Parse(cakeStr[0]),cakeStr[1], cakeStr[2]));
                }
            }

            return result;
        }

        public static int GetCurrentId()
        {
            return File.ReadAllText(DbPath).Split(Environment.NewLine).Length;
        }

        public static List<Cake> ListAllCakes()
        {
            string[] cakesFromFile = File.ReadAllText(DbPath).Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var line in cakesFromFile)
            {
                string[] cakeStr = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (cakeStr.Length != 3)
                {
                    continue;
                }

                cakes.Add(new Cake(int.Parse(cakeStr[0]), cakeStr[1], cakeStr[2]));
            }

            return cakes;
        }

        public static Cake GetCakeById(int id)
        {
            cakes = ListAllCakes();

            return cakes.FirstOrDefault(c => c.Id == id);
        }
    }
}
