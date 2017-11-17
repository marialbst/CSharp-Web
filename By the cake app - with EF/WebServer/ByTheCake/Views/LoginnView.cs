namespace WebServer.ByTheCake.Views
{
    using Server.Contracts;
    using System.IO;

    public class LoginnView : IView
    {
        private const string Path = @"ByTheCake/Resources/login.html";
        private const string ResultPlaceholder = "{{{result}}}";

        private string replacement;
        private string username;
        private string password;

        public LoginnView()
        {
        }

        public LoginnView(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string View()
        {
            string html = File.ReadAllText(Path);

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                replacement = $"Hello {this.username}. Your password is {this.password}";
            }
            else
            {
                replacement = string.Empty;
            }

            return html.Replace(ResultPlaceholder, replacement);
        }
    }
}
