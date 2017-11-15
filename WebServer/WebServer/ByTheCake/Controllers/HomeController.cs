namespace WebServer.ByTheCake.Controllers
{
    using Views;
    using Server.Enums;
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;

    public class HomeController
    {
        public IHttpResponse Index()
        {
            return new ViewResponse(HttpStatusCode.Ok, new IndexView());
        }

        public IHttpResponse About()
        {
            return new ViewResponse(HttpStatusCode.Ok, new AboutView());
        }

    }
}
