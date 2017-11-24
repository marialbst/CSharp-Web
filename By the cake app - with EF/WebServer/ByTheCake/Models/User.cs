namespace WebServer.ByTheCake.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public IEnumerable<Order> Orders { get; set; } = new List<Order>();
    }
}
