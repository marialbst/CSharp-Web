namespace WebServer.Application.Controllers
{
    using Views;
    using Server;
    using Server.Enums;
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;

    public class UserController
    {
        public IHttpResponse RegisterGet()
        {
            return new ViewResponse(HttpStatusCode.Ok, new RegisterView());
        }

        public IHttpResponse RegisterPost(string name)
        {
            return new RedirectResponse($"/user/{name}");
        }

        public IHttpResponse Details(string name)
        {
            Model model = new Model { ["name"] = name };

            return new ViewResponse(HttpStatusCode.Ok, new UserDetailsView(model)); ;
        }
    }
}
