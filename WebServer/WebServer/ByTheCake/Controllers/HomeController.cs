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
            string user = "";

            if (session.Contains(SessionStore.CurrentUserKey))
            {
                user = session.Get(SessionStore.CurrentUserKey).ToString();
            }
            return new ViewResponse(HttpStatusCode.Ok, new IndexView(user));
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
