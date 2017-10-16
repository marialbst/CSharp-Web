namespace WebServer.Application.Views
{
    using Server.Contracts;

    public class HomeIndexView : IView
    {
        public string View()
        {
            return "<body><h2>Wellcome<h2></body>";
        }
    }
}
