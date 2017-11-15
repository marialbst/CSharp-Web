namespace WebServer.ByTheCake.Views
{
    using Server.Contracts;
    using System.IO;
    using WebServer.ByTheCake.Models;

    public class AddCakeView : IView
    {
        private const string Path = @"ByTheCake/Resources/add.html";
        private const string ResultPlaceholder = "{{{result}}}";

        private string type;
        private Cake cake;

        public AddCakeView()
        {
        }

        public AddCakeView(string type)
        {
            this.type = type;
        }

        public AddCakeView(Cake cake)
        {
            this.cake = cake;
        }

        public string View()
        {
            string html = File.ReadAllText(Path);
            string replacement = "";

            if (string.IsNullOrWhiteSpace(type))
            {
                replacement = string.Empty;
            }

            if (type == "error")
            {
                replacement = "<div class=\"red\">Invalid name or price</div>";
            }

            if (cake != null)
            {
                replacement = cake.ToString();
            }

            return html.Replace(ResultPlaceholder, replacement);
        }
    }
}
