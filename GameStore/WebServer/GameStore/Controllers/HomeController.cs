namespace WebServer.GameStore.Controllers
{
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;

    public class HomeController : Controller
    {
        private const string IndexPath = @"Home\index";

        public HomeController(IHttpRequest request)
            :base(request)
        {

        }

        public IHttpResponse Index()
        {
            return this.FileViewResponse(IndexPath);
        }

        public IHttpResponse Image(string imagePath)
        {
            return new ImageResponse(imagePath);
        }
    }
}
