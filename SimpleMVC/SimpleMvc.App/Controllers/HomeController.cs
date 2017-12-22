namespace SimpleMvc.App.Controllers
{
    using Framework.Contracts;
    using Framework.Controllers;
    using Framework.Attributes.Methods;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }

        //GET /home/create/
        public void Create()
        {

        }
        
        [HttpPost]
        public void Create(int id)
        {

        }
    }
}
