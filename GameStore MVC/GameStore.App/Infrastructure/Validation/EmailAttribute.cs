namespace GameStore.App.Infrastructure.Validation
{
    using SimpleMvc.Framework.Attributes.Property;

    public class EmailAttribute : PropertyAttribute
    {
        public override bool IsValid(object value)
        {
            string email = value as string;

            if (email == null)
            {
                return true;
            }

            return email.Contains(".") && email.Contains("@");
        }
    }
}
