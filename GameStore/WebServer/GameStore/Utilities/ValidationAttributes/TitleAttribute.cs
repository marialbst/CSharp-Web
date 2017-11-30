namespace WebServer.GameStore.Utilities.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    public class TitleAttribute : ValidationAttribute
    {
        public TitleAttribute()
        {
            this.ErrorMessage = ValidationConstants.FirstLetterTitleError;
        }

        public override bool IsValid(object value)
        {
            var title = value as string;

            if (title == null)
            {
                return true;
            }

            return char.IsUpper(title[0]);
        }
    }
}
