namespace SimpleMvc.App.Controllers
{
    using Framework.Contracts;
    using Framework.Controllers;
    using Framework.Attributes.Methods;

    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
