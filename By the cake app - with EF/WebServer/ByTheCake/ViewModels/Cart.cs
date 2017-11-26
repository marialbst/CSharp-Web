namespace WebServer.ByTheCake.ViewModels
{
    using System.Collections.Generic;

    public class Cart
    {
        public const string SessionKey = "shopping cart";

        public ICollection<int> ProductIds { get; set; } = new List<int>();
    }
}
