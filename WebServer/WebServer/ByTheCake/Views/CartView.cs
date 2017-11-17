namespace WebServer.ByTheCake.Views
{
    using Server.Contracts;
    using Models;
    using System.IO;
    using System.Text;

    public class CartView : IView
    {
        private const string Path = @"ByTheCake/Resources/cart.html";
        private const string ProductsPlaceholder = "{{{products}}}";
        private const string PricePlaceholder = "{{{price}}}";

        private Cart cart;

        public CartView(Cart cart)
        {
            this.cart = cart;
        }

        public string View()
        {
            string html = File.ReadAllText(Path);

            string productsReplacement = this.FormatReplacement();
            string priceReplacement = $"${this.cart.TotalCost():f2}";

            string res = html.Replace(ProductsPlaceholder, productsReplacement);
            return res.Replace(PricePlaceholder, priceReplacement);
        }

        private string FormatReplacement()
        {
            StringBuilder prRepl = new StringBuilder();

            foreach (var cake in this.cart.Cakes)
            {
                prRepl.AppendLine($"<p>{cake.Name} ${cake.Price:f2}</p>");
            }

            return prRepl.ToString();
        }
    }
}
