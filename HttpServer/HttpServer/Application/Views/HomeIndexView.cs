namespace HttpServer.Application.Views
{
    using Server.Contracts;

    public class HomeIndexView : IView
    {
        public string View()
        {
            string html = "<body><h1>Hello</h1></body>";

            return html;
        }
    }
}
