namespace HttpServer.Application.Views
{
    using Server.Contracts;

    public class RegisterView : IView
    {
        public string View()
        {
            string html = "<body><form method=\"POST\">Name<br/>";
            html += "<input type=\"text\" name=\"name\" /><br/>";
            html += "<input type=\"submit\" /></form></body>";

            return html;
        }
    }
}
