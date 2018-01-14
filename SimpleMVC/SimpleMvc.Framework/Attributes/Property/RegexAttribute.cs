namespace SimpleMvc.Framework.Attributes.Property
{
    using System.Text.RegularExpressions;

    public class RegexAttribute : PropertyAttribute
    {
        private readonly string pattern;

        public RegexAttribute(string pattern)
        {
            this.pattern = $"^{pattern}$";
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;

            if (valueAsString == null)
            {
                return true;
            }

            return Regex.IsMatch(valueAsString, this.pattern);
        }
    }
}
