namespace HttpServer.Application.Controllers
{
    using Server.HTTP.Response;
    using Server.HTTP.Contracts;
    using Server.Enums;
    using Views;

    public class HomeController
    {
        // GET /
        public IHttpResponse Index()
        {
            return new ViewResponse(HttpResponseStatusCode.Ok, new HomeIndexView());
        }
    }
}
