namespace WebServer.ByTheCake.Controllers
{
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;

    public class HomeController : Controller
    {
        private const string IndexPath = @"Home\index";
        private const string AboutPath = @"Home\about";

        public IHttpResponse Index()
        {
            return this.FileViewResponse(IndexPath);
        }

        public IHttpResponse About()
        {
            return this.FileViewResponse(AboutPath);
        }
        
        public IHttpResponse Image(string imagePath)
        {
            return new ImageResponse(imagePath);
        }
    }
}
