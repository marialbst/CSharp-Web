namespace WebServer.GameStore.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Game;

    public class Cart
    {
        public HashSet<IndexViewGame> Games { get; set; } = new HashSet<IndexViewGame>();

        public decimal TotalPrice
        {
            get => this.Games.Sum(g => g.Price);
        }
    }
}
