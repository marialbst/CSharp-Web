namespace WebServer.ByTheCake.Views
{
    using Server.Contracts;
    using System.IO;

    public class SuccessView : IView
    {
        private const string Path = @"ByTheCake/Resources/success.html";
        private const string MessagePlaceholder = "{{{message}}}";

        private string message;

        public SuccessView(string message)
        {
            this.message = message;
        }

        public string View()
        {
            string html = File.ReadAllText(Path);
            return html.Replace(MessagePlaceholder, message);
        }
    }
}
