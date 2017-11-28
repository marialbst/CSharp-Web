namespace WebServer.GameStore.Controllers
{
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;

    public class HomeController : Controller
    {
        private const string IndexPath = @"Home\index";

        public IHttpResponse Index(IHttpSession session)
        {
            this.GetUserType(session);

            return this.FileViewResponse(IndexPath);
        }

        public IHttpResponse Image(string imagePath)
        {
            return new ImageResponse(imagePath);
        }
    }
}
