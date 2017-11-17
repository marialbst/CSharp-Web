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
        private string wordToSearch;
        private int cartProductsCount;

        private List<Cake> cakes;

        public SearchCakeView(int products, string type = "")
        {
            this.type = type;
            this.cartProductsCount = products;
        }
        
        public SearchCakeView(int products, List<Cake> cakes, string wordToSearch)
        {
            this.cakes = cakes;
            this.wordToSearch = wordToSearch;
            this.cartProductsCount = products;
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
                        if (!string.IsNullOrEmpty(this.wordToSearch))
                        {
                            result.AppendLine($"<div>{cake.Name} ${cake.Price:f2}<a href=\"/order?id={cake.Id}&returnUrl={this.wordToSearch}\"><button>Order</button></a></div>");
                        }
                        else
                        {
                            result.AppendLine($"<div>{cake.Name} ${cake.Price:f2}<a href=\"/order?id={cake.Id}\"><button>Order</button></a></div>");
                        }
                    }

                    replacement = result.ToString();
                }
                else
                {
                    replacement = "<h3 class=\"red\">No cakes found by given criteria</h3>";
                }
            }
            //ToDo replace with real cart count
            string products = this.cartProductsCount != 1 ? $"{cartProductsCount} products" : $"{cartProductsCount} product";
            string res = html.Replace(ProductsPlaceholder, products);
            return res.Replace(ResultPlaceholder, replacement);
        }
    }
}
