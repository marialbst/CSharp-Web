namespace GameStore.App.Models.Users
{
    using Infrastructure.Validation;

    public class RegisterModel
    {
        [Email]
        [Required]
        public string Email { get; set; }

        public string Name { get; set; }

        [Required]
        [Password]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}
