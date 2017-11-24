namespace WebServer.ByTheCake.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public IEnumerable<OrderProduct> Orders { get; set; } = new List<OrderProduct>();
    }
}
