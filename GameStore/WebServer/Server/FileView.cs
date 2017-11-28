namespace WebServer.Server
{
    using Contracts;

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
