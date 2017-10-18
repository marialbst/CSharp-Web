namespace WebServer.ByTheCakeApp.Controllers
{
    using Helpers;
    using Models;
    using System.Collections.Generic;
    using Server.HTTP.Contracts;
    using System.Text;
    using System.IO;

    public class CakeController : Controller
    {
        private string Path = @"ByTheCakeApp\Data\database.csv";
        private static readonly List<Cake> cakes = new List<Cake>();

        public IHttpResponse AddGet()
        {
            return this.FileViewResponse(@"cakes\add");
        }

        public IHttpResponse AddPost(string name, string price)
        {
            Cake cake = new Cake(name, price);
            cakes.Add(cake);

            using(var streamWriter = new StreamWriter(Path, true))
            {
                streamWriter.WriteLine($"{name},{price}");
            }

            var result = new Dictionary<string, string>
            {
                ["name"] = name,
                ["price"] = price
            };

            return this.FileViewResponse(@"cakes\add", result);
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

        private Dictionary<string, string> SearchResults(string name)
        {
            var result = new Dictionary<string, string>();

            string[] textFileLines = File.ReadAllLines(Path);

            foreach (var line in textFileLines)
            {
                if (!line.ToLower().Contains(name))
                {
                    continue;
                }

                string[] cakeParams = line.Split(',');

                if (cakeParams.Length != 2)
                {
                    continue;
                }

                result.Add(cakeParams[0], $"${cakeParams[1]}");
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
