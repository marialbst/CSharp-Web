namespace WebServer.GameStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        
        public string FullName { get; set; }

        public ICollection<UserGame> Games { get; set; } = new List<UserGame>();

        public bool IsAdmin { get; set; }
    }
}
