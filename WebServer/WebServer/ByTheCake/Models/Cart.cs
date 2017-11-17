namespace WebServer.ByTheCake.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class Cart
    {
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
