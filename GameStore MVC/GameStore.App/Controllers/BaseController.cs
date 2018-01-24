namespace GameStore.App.Controllers
{
    using GameStore.Services;
    using GameStore.Services.Contracts;
    using SimpleMvc.Framework.Controllers;

    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            this.Model["show-error"] = "hide";
            this.Model["error-message"] = string.Empty;

            this.UserService = new UserService();
        }

        protected IUserService UserService { get; private set; }

        protected void ShowError(string message)
        {
            this.Model["show-error"] = "show";
            this.Model["error-message"] = message;
        }
    }
}
