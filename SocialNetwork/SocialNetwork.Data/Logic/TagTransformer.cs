namespace SocialNetwork.Data.Logic
{
    public static class TagTransformer
    {
        public static string TransformTag(string tag)
        {
            string result = tag.Replace(" ", string.Empty);

            if (!result.StartsWith("#"))
            {
                result = "#" + result;
            }

            return result.Length > 20 ? result.Substring(0, 20) : result;
        }
    }
}
