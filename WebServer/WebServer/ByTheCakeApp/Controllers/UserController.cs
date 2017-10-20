namespace WebServer.ByTheCakeApp.Controllers
{
    using Server.Enums;
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;
    using Views;

    public class UserController
    {
        public IHttpResponse LoginGet()
        {
            return new ViewResponse(HttpStatusCode.Ok, new LoginView());
        }

        public IHttpResponse LoginPost(string name, string password)
        {
            return new ViewResponse(HttpStatusCode.Ok, new LoginView(name, password));
        }
    }
}
