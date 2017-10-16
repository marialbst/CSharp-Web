namespace HttpServer.Application.Controllers
{
    using Server.HTTP.Response;
    using Server.HTTP.Contracts;
    using Server.Enums;
    using Views;
    using Server;

    public class UserController
    {
        public IHttpResponse RegisterGet()
        {
            return new ViewResponse(HttpResponseStatusCode.Ok, new RegisterView());
        }

        public IHttpResponse RegisterPost(string name)
        {
            return new RedirectResponse($"user/{name}");
        }

        public IHttpResponse UserDetails(string name)
        {
            Model model = new Model { ["name"] = name };
            return new ViewResponse(HttpResponseStatusCode.Ok, new UserDetailsView(model));
        }
    }
}
