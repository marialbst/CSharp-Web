namespace WebServer.ByTheCake.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    public class Cart
    {
        public const string SessionKey = "shopping cart";

        public Cart()
        {
            this.Cakes = new List<Cake>();
        }

        public ICollection<Cake> Cakes { get; set; }

        public decimal TotalCost()
        {
            return this.Cakes.Select(c => c.Price).Sum();
        }
    }
}
