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
            File.AppendAllText(DbPath, $"{cake.Name},{cake.Price}{Environment.NewLine}");
        }

        public static List<Cake> SearchCakes(string word)
        {
            List<Cake> result = new List<Cake>();
            string[] cakesFromFile = File.ReadAllText(DbPath).Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in cakesFromFile)
            {
                string[] cakeStr = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (cakeStr.Length != 2)
                {
                    continue;
                }

                if (cakeStr[0].ToLower().Contains(word.ToLower()))
                {
                    result.Add(new Cake(cakeStr[0], cakeStr[1]));
                }
            }

            return result;
        }

        public static List<Cake> ListAllCakes()
        {
            string[] cakesFromFile = File.ReadAllText(DbPath).Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in cakesFromFile)
            {
                string[] cakeStr = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (cakeStr.Length != 2)
                {
                    continue;
                }

                    cakes.Add(new Cake(cakeStr[0], cakeStr[1]));
            }

            return cakes;
        }
    }
}
