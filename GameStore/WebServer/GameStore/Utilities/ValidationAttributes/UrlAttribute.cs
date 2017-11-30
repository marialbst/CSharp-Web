namespace WebServer.GameStore.Utilities.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    public class ImageUrlAttribute : ValidationAttribute
    {
        public ImageUrlAttribute()
        {
            this.ErrorMessage = ValidationConstants.InvalidImageUrl;
        }

        public override bool IsValid(object value)
        {
            var url = value as string;
            
            if (url == null)
            {
                return true;
            }

            var urlToLower = url.ToLower();

            return urlToLower.StartsWith("http://")
                || urlToLower.StartsWith("https://")
                || urlToLower == "null";
        }
    }
}
