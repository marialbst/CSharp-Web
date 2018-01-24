namespace GameStore.App.Controllers
{
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;
    using GameStore.App.Models.Users;
    using SimpleMvc.Framework.ActionResults;
    using GameStore.App.Infrastructure;

    public class UsersController : BaseController
    {
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (model.Password != model.ConfirmPassword
                || !this.IsValidModel(model))
            {
                this.ShowError(ErrorConstants.RegisterError);
                
                return this.View();
            }

            bool result = this.UserService.Create(model.Email, model.Password, model.Name);

            if (!result)
            {
                this.ShowError(ErrorConstants.ExistingUserError);

                return this.View();
            }

            this.SignIn(model.Email);

            return new RedirectResult("/");
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (!this.IsValidModel(model))
            {
                this.ShowError(ErrorConstants.LoginError);

                return this.View();
            }

            bool result = this.UserService.Find(model.Email, model.Password);

            if (!result)
            {
                this.ShowError(ErrorConstants.InvalidCredentials);

                return this.View();
            }

            this.SignIn(model.Email);

            return new RedirectResult("/");
        }

        public IActionResult Logout()
        {
            this.SignOut();

            return new RedirectResult("/");
        }
    }
}
