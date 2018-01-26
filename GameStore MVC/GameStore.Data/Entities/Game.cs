namespace GameStore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Game
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3),MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MinLength(11),MaxLength(11)]
        public string TrailerId { get; set; }
        
        public string ImageThumbnail { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Size { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [MinLength(20)]
        public string Description { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        public ICollection<Order> Users { get; set; } = new List<Order>();
    }
}
