namespace WebServer.GameStore.Utilities.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    public class EmailAttribute : ValidationAttribute
    {
        public EmailAttribute()
        {
            this.ErrorMessage = ValidationConstants.EmailError;
        }

        public override bool IsValid(object value)
        {
            var email = value as string;

            if (email == null)
            {
                return true;
            }

            return email.Contains("@") && email.Contains(".");
        }
    }
}
