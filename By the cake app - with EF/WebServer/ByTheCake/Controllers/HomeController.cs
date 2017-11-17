namespace WebServer.ByTheCake.Controllers
{
    using Views;
    using Server.Enums;
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;
    using Server.HTTP;

    public class HomeController
    {
        public IHttpResponse Index(IHttpSession session)
        {
            return new ViewResponse(HttpStatusCode.Ok, new IndexView(session.Get(SessionStore.CurrentUserKey).ToString()));
        }

        public IHttpResponse About()
        {
            return new ViewResponse(HttpStatusCode.Ok, new AboutView());
        }

        public IHttpResponse Image(string imagePath)
        {
            return new ImageResponse(imagePath);
        }
    }
}
