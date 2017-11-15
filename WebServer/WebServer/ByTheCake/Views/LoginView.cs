namespace WebServer.ByTheCake.Views
{
    using Server.Contracts;
    using System.IO;

    public class LoginView : IView
    {
        private const string Path = @"ByTheCake/Resources/login.html";
        private const string ResultPlaceholder = "{{{result}}}";

        private bool isError;

        public LoginView(bool isError = false)
        {
            this.isError = isError;
        }

        public string View()
        {
            string html = File.ReadAllText(Path);
            string replacement = string.Empty;

            if (this.isError)
            {
                replacement = "<div class=\"red\">Invalid username or password</div>";
            }

            return html.Replace(ResultPlaceholder, replacement);
        }
    }
}
