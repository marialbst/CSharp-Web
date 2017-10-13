namespace SocialNetwork.Data
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Validations;

    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [Tag]
        [MaxLength(20)]
        public string Name { get; set; }

        public List<AlbumTag> Albums { get; set; } = new List<AlbumTag>();
    }
}
