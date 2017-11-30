namespace WebServer.GameStore.ViewModels.Game
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Utilities;
    using Utilities.ValidationAttributes;

    public class ManageGameViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Title]
        [MinLength(ValidationConstants.Game.TitleMinLength,
           ErrorMessage = ValidationConstants.InvalidMinLengthError)]
        [MaxLength(ValidationConstants.Game.TitleMaxLength,
           ErrorMessage = ValidationConstants.InvalidMaxLengthError)]
        public string Title { get; set; }

        [Display(Name = "Youtube video Id")]
        [Required]
        [MinLength(ValidationConstants.Game.TrailerIdLength,
             ErrorMessage = ValidationConstants.InvalidLengthError)]
        [MaxLength(ValidationConstants.Game.TrailerIdLength,
            ErrorMessage = ValidationConstants.InvalidLengthError)]
        public string TrailerId { get; set; }

        [Required]
        [ImageUrl]
        public string ImageThumbnail { get; set; }

        [Required]
        [Range(0, double.MaxValue,
            ErrorMessage = ValidationConstants.InvalidNumberError)]
        public double Size { get; set; }

        [Required]
        [Range(0, float.MaxValue,
            ErrorMessage = ValidationConstants.InvalidNumberError)]
        public decimal Price { get; set; }

        [Required]
        [MinLength(ValidationConstants.Game.DescriptionMinLength,
             ErrorMessage = ValidationConstants.InvalidMinLengthError)]
        public string Description { get; set; }

        [Display(Name = "Release date")]
        [Required]
        public DateTime? ReleaseDate { get; set; }
    }
}
