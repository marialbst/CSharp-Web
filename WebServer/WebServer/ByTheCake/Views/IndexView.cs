namespace WebServer.ByTheCake.Views
{
    using System.IO;
    using Server.Contracts;

    public class IndexView : IView
    {
        private const string Path = @"ByTheCake/Resources/index.html";

        public string View()
        {
            return File.ReadAllText(Path);
        }
    }
}
