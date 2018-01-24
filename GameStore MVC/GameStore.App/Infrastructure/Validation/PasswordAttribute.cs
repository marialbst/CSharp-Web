namespace GameStore.App.Infrastructure.Validation
{
    using System.Linq;
    using SimpleMvc.Framework.Attributes.Property;

    public class PasswordAttribute : PropertyAttribute
    {
        public override bool IsValid(object value)
        {
            string password = value as string;

            if (password == null)
            {
                return true;
            }

            return password.Any(c => char.IsUpper(c)) 
                && password.Any(c => char.IsLower(c)) 
                && password.Any(c => char.IsNumber(c)) 
                && password.Length >= 6;
        }
    }
}
