namespace WebServer.ByTheCake.Views
{
    using Server.Contracts;

    public class FileView : IView
    {
        private string htmlFile;

        public FileView(string htmlFile)
        {
            this.htmlFile = htmlFile;
        }

        public string View()
        {
            return this.htmlFile;
        }
    }
}
