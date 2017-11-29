namespace WebServer.GameStore.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;
    using Utilities;
    using Utilities.ValidationAttributes;

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(ValidationConstants.Account.EmailMaxLength,
            ErrorMessage = ValidationConstants.InvalidMaxLengthError)]
        public string Email { get; set; }

        [Required]
        [Display( Name = "Full name")]
        [MinLength(ValidationConstants.Account.FullNameMinLength, 
            ErrorMessage = ValidationConstants.InvalidMinLengthError)]
        [MaxLength(ValidationConstants.Account.FullNameMaxLength,
            ErrorMessage = ValidationConstants.InvalidMaxLengthError)]
        public string FullName { get; set; }

        [Required]
        [Password]
        [MinLength(ValidationConstants.Account.PasswordMinLength,
            ErrorMessage = ValidationConstants.InvalidMinLengthError)]
        [MaxLength(ValidationConstants.Account.PasswordMaxLength,
            ErrorMessage = ValidationConstants.InvalidMaxLengthError)]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
