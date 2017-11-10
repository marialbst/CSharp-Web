namespace WebServer.ByTheCakeApp.Views
{
    using System.IO;
    using Server.Contracts;

    public class OkView : IView
    {
        private const string DefaultPath = @"ByTheCakeApp\Resources\Home\success.html";

        public string View()
        {
            return File.ReadAllText(DefaultPath);
        }
    }
}
