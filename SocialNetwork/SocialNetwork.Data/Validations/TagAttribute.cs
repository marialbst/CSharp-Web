namespace SocialNetwork.Data.Validations
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Logic;

    public class TagAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string tag = value.ToString();

            if (tag == null)
            {
                return true;
            }

            return tag.StartsWith("#") && tag.All(c => !char.IsWhiteSpace(c)) && tag.Length <= 20;
        }
    }
}
