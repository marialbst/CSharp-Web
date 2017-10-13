namespace SocialNetwork.Data
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Album
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Background { get; set; }

        public bool IsPublic { get; set; }

        public int OwnerId { get; set; }

        public User Owner { get; set; }

        public List<AlbumPicture> Pictures { get; set; } = new List<AlbumPicture>();

        public List<AlbumTag> Tags { get; set; } = new List<AlbumTag>();
    }
}
