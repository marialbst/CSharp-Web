namespace WebServer.ByTheCake.Views
{
    using System.IO;
    using Server.Contracts;

    public class IndexView : IView
    {
        private const string Path = @"ByTheCake/Resources/index.html";
        private const string UserPlaceholder = "{{{user}}}";

        private string username;

        public IndexView(string username)
        {
            this.username = username;
        }

        public string View()
        {
            string html = File.ReadAllText(Path);
            return html.Replace(UserPlaceholder, username);
        }
    }
}
