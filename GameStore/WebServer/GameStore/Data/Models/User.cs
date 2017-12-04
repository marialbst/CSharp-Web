namespace WebServer.GameStore.Data.Models
{
    using Utilities;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            this.Games = new List<UserGame>();
        }

        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(ValidationConstants.Account.EmailMaxLength)]
        public string Email { get; set; }

        [Required]
        [MinLength(ValidationConstants.Account.PasswordMinLength)]
        [MaxLength(ValidationConstants.Account.PasswordMaxLength)]
        public string Password { get; set; }

        [MinLength(ValidationConstants.Account.FullNameMinLength)]
        [MaxLength(ValidationConstants.Account.FullNameMaxLength)]
        public string FullName { get; set; }

        public ICollection<UserGame> Games { get; set; }

        public bool IsAdmin { get; set; }
    }
}
