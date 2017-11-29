namespace WebServer.GameStore.Utilities.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class PasswordAttribute : ValidationAttribute
    {
        public PasswordAttribute()
        {
            this.ErrorMessage = ValidationConstants.EmailError;
        }

        public override bool IsValid(object value)
        {
            var password = value as string;

            if (password == null)
            {
                return true;
            }

            return password.Any(ch => char.IsUpper(ch))
                && password.Any(ch => char.IsLower(ch))
                && password.Any(ch => char.IsDigit(ch));
        }
    }
}
