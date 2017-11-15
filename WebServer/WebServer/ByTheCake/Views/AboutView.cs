namespace WebServer.ByTheCake.Views
{
    using Server.Contracts;
    using System.IO;

    public class AboutView : IView
    {
        private const string Path = @"ByTheCake/Resources/about.html";

        public string View()
        {
            return File.ReadAllText(Path);
        }
    }
}
