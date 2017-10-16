namespace WebServer.Application.Views
{
    using Server.Contracts;

    public class RegisterView : IView
    {
        public string View()
        {
            string result = "<body><form method=\"POST\"><input type=\"text\" name=\"name\"/><input type=\"submit\"/></form></body>";
            return result;
        }
    }
}
