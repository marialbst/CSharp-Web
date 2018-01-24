namespace GameStore.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        public string Password { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        //public ICollection<Order> Games { get; set; } = new List<Order>();

        public bool IdAdmin { get; set; }
    }
}
