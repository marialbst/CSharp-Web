namespace WebServer.ByTheCake.Views
{
    using Models;
    using Server.Contracts;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class SearchCakeView : IView
    {
        private const string Path = @"ByTheCake/Resources/search.html";
        private const string ResultPlaceholder = "{{{result}}}";
        private const string ProductsPlaceholder = "{{{products}}}";

        private string type;
        private List<Cake> cakes;

        public SearchCakeView(string type = "")
        {
            this.type = type;
        }
        
        public SearchCakeView(List<Cake> cakes)
        {
            this.cakes = cakes;
        }

        public string View()
        {
            string html = File.ReadAllText(Path);
            string replacement = "";

            if (string.IsNullOrWhiteSpace(type))
            {
                replacement = string.Empty;
            }

            if (type == "error")
            {
                replacement = "<h3 class=\"red\">No cakes found by given criteria</h3>";
            }

            if (cakes != null)
            {
                if (cakes.Count > 0)
                {
                    StringBuilder result = new StringBuilder();

                    foreach (var cake in cakes)
                    {
                        result.Append($"<span>{cake.Name} ${cake.Price:f2}</span>");
                        result.AppendLine($"<button>Order</button><br />");
                    }

                    replacement = result.ToString();
                }
                else
                {
                    replacement = "<h3 class=\"red\">No cakes found by given criteria</h3>";
                }
            }
            //ToDo replace with real cart count
            string res = html.Replace(ProductsPlaceholder, "0 products");
            return res.Replace(ResultPlaceholder, replacement);
        }
    }
}
