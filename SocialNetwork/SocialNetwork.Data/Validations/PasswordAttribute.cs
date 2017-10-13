namespace SocialNetwork.Data.Validations
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class PasswordAttribute : ValidationAttribute
    {
        private readonly char[] AlowedSymbols = new[] {'!','@','#','$','%','^','&','*','(',')','_','+','>','<', '?' };

        public PasswordAttribute()
        {
            this.ErrorMessage = "Password is not valid.";
        }

        public override bool IsValid(object value)
        {
            var password = value.ToString();

            if (password == null)
            {
                return true;
            }

            return password.All(p => char.IsUpper(p) || char.IsLower(p) || char.IsDigit(p) || AlowedSymbols.Contains(p));
        }
    }
}
