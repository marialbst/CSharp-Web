namespace WebServer.GameStore.Controllers
{
    using Server.HTTP.Contracts;
    using ViewModels.Account;
    using Service;
    using Service.Contracts;
    using Utilities;
    using Server.HTTP.Response;
    using Server.HTTP;

    public class AccountController : Controller
    {
        private const string RegisterPath = @"Account\register";
        private const string LoginPath = @"Account\login";

        private IUserService userService;

        public AccountController(IHttpRequest request)
            :base(request)
        {
            this.userService = new UserService();
        }

        public IHttpResponse Register()
        {
            if (this.IsAuthenticated)
            {
                return new RedirectResponse("/");
            }

            return this.FileViewResponse(RegisterPath);
        }

        public IHttpResponse Register(RegisterViewModel model)
        {
            if (!this.ValidateModel(model))
            {
                return this.FileViewResponse(RegisterPath);
            }

            if (!this.userService.Save(model.Email, model.Password, model.FullName))
            {
                this.AddError(ValidationConstants.UsernameTakenError);

                return this.FileViewResponse(RegisterPath);
            }

            this.Request.Session.Add(SessionStore.CurrentUserKey, model.Email);
            return new RedirectResponse("/");
        }

        public IHttpResponse Login()
        {
            if (this.IsAuthenticated)
            {
                return new RedirectResponse("/");
            }

            return this.FileViewResponse(LoginPath);
        }

        public IHttpResponse Login(LoginViewModel model)
        {
            if (!this.ValidateModel(model))
            {
                return this.FileViewResponse(LoginPath);
            }

            bool result = this.userService.Find(model.Email, model.Password);

            if (!result)
            {
                this.AddError(ValidationConstants.EmailOrPasswordError);

                return this.FileViewResponse(LoginPath);
            }

            this.Request.Session.Add(SessionStore.CurrentUserKey, model.Email);
            return new RedirectResponse("/");
        }

        public IHttpResponse Logout()
        {
            if (!this.IsAuthenticated)
            {
                return new RedirectResponse("/login");
            }

            this.Request.Session.Clear(SessionStore.CurrentUserKey);

            return new RedirectResponse("/");
        }
    }
}
