namespace GameStore.App.Controllers
{
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;
    using Models.Users;
    using Infrastructure;

    public class UsersController : BaseController
    {
        public IActionResult Register()
        {
            if (this.User.IsAuthenticated)
            {
                return this.RedirectToAction(IndexPath);
            }

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

            return this.RedirectToAction(LoginPath);
        }

        public IActionResult Login()
        {
            if (this.User.IsAuthenticated)
            {
                return this.RedirectToAction(IndexPath);
            }

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

            return this.RedirectToAction(IndexPath);
        }

        public IActionResult Logout()
        {
            if (this.User.IsAuthenticated)
            {
                this.SignOut();
            }

            return this.RedirectToAction(IndexPath);
        }
    }
}
