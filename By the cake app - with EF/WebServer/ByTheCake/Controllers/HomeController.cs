namespace WebServer.ByTheCake.Controllers
{
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;

    public class HomeController : Controller
    {
        public IHttpResponse Index()
        {
            return this.FileViewResponse(@"home\index");
        }

        public IHttpResponse About()
        {
            return this.FileViewResponse(@"home\about");
        }
        
        public IHttpResponse Image(string imagePath)
        {
            return new ImageResponse(imagePath);
        }
    }
}
