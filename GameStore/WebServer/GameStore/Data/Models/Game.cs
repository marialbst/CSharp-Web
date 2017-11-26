namespace WebServer.GameStore.Data.Models
{
    using System;

    public class Game
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Trailer { get; set; }

        public string ImageThumbnail { get; set; }

        public int Size { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
