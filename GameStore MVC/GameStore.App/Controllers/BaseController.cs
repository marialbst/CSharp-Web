namespace GameStore.App.Controllers
{
    using GameStore.Services;
    using GameStore.Services.Contracts;
    using SimpleMvc.Framework.Controllers;

    public abstract class BaseController : Controller
    {
        protected const string LoginPath = "/users/login";
        protected const string IndexPath = "/";

        protected BaseController()
        {
            this.ViewModel["show-error"] = "hide";
            this.ViewModel["error-message"] = string.Empty;

            this.ViewModel["guest-menu"] = "show";
            this.ViewModel["user-menu"] = "hide";
            this.ViewModel["admin-menu"] = "hide";

            this.UserService = new UserService();
            this.GameService = new GameService();
        }

        protected override void InitializeController()
        {
            base.InitializeController();

            if (this.User.IsAuthenticated 
                && this.UserService.Exists(this.User.Name))
            {
                this.ViewModel["guest-menu"] = "hide";
                this.ViewModel["user-menu"] = "show";

                if (this.UserIsAdmin)
                {
                    this.ViewModel["admin-menu"] = "show";
                }
            }
        }
        
        protected IUserService UserService { get; private set; }

        protected IGameService GameService { get; private set; }

        protected void ShowError(string message)
        {
            this.ViewModel["show-error"] = "show";
            this.ViewModel["error-message"] = message;
        }

        protected bool UserIsAdmin
            => this.UserService.IsAdmin(this.User.Name);
    }
}
