namespace WebServer.ByTheCakeApp.Controllers
{
    using Server.HTTP.Contracts;
    using Helpers;

    public class HomeController : Controller
    {
        public IHttpResponse Index()
        {
            return this.FileViewResponse(@"Home\index");
        }

        public IHttpResponse About()
        {
            return this.FileViewResponse(@"Home\about");
        }
    }
}
