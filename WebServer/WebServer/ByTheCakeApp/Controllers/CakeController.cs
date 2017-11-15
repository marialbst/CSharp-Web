namespace WebServer.ByTheCakeApp.Controllers
{
    using Helpers;
    using Models;
    using System.Collections.Generic;
    using Server.HTTP.Contracts;
    using System.Text;
    using System.IO;
    using System;

    public class CakeController : Controller
    {
        private string DbPath = @"ByTheCakeApp\Data\database.csv";
        private static readonly List<Cake> cakes = new List<Cake>();

        public IHttpResponse AddGet()
        {
            return this.FileViewResponse(@"cakes\add");
        }

        public IHttpResponse AddPost(string name, string price)
        {   
            if (!File.Exists(DbPath))
            {
                File.Create(DbPath);
            }

            var streamReader = new StreamReader(DbPath);
            var id = streamReader.ReadToEnd().Split(Environment.NewLine).Length;
            streamReader.Dispose();

            using (var streamWriter = new StreamWriter(DbPath, true))
            {
                Cake cake = new Cake(id, name, price);
                cakes.Add(cake);
                streamWriter.WriteLine($"{id},{name},{price}");
            }

            return this.FileViewResponse(@"cakes\add", cakes);
        }

        public IHttpResponse Search(Dictionary<string, string> urlParameters)
        {
            if (urlParameters.ContainsKey("name"))
            {
                string name = urlParameters["name"];
                return this.FileViewResponse(@"cakes\search", this.SearchResults(name.ToLower()));
            }

            return this.FileViewResponse(@"cakes\search");
        }

        private List<Cake> SearchResults(string name)
        {
            var result = new List<Cake>();

            string[] textFileLines = File.ReadAllLines(DbPath);

            foreach (var line in textFileLines)
            {
                if (!line.ToLower().Contains(name))
                {
                    continue;
                }

                string[] cakeParams = line.Split(',');

                if (cakeParams.Length != 3)
                {
                    continue;
                }

                result.Add(new Cake(int.Parse(cakeParams[0]),cakeParams[1], $"{cakeParams[2]}"));
            }

            return result;
        }

        private string AddCakeReplacement(string name, string price)
        {
            StringBuilder replacement = new StringBuilder();
            replacement.AppendLine($"<div>name: {name}</div>");
            replacement.AppendLine($"<div>price: {price}</div>");
            return replacement.ToString();
        }
    }
}
