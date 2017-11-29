namespace WebServer.GameStore.ViewModels.Game
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Utilities;

    public class AddGameViewModel
    {
        [Required]
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
        public string ImageThumbnail { get; set; }

        public double Size { get; set; }

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
