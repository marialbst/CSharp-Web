namespace WebServer.ByTheCake.Models
{
    using System;
    using System.Collections.Generic;

    public class Order
    {
        public int Id { get; set; }

        public DateTime CreationTime { get; set; }

        public IEnumerable<OrderProduct> Products { get; set; } = new List<OrderProduct>();

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
