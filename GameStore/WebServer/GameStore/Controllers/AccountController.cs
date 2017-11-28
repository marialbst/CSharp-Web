namespace WebServer.GameStore.Controllers
{
    using Server.HTTP.Contracts;
    using ViewModels.Account;
    using Service;
    using Service.Contracts;
    using Validation;
    using WebServer.Server.HTTP.Response;
    using WebServer.Server.HTTP;

    public class AccountController : Controller
    {
        private const string RegisterPath = @"Account\register";
        private const string LoginPath = @"Account\login";

        private IUserService userService;

        public AccountController()
        {
            this.userService = new UserService();
        }

        public IHttpResponse Register(IHttpSession session)
        {
            this.GetUserType(session);
            this.ViewData["showError"] = Controller.HideElementClass;

            if (session.Contains(SessionStore.CurrentUserKey))
            {
                return new RedirectResponse("/");
            }

            return this.FileViewResponse(RegisterPath);
        }

        public IHttpResponse Register(IHttpRequest req, RegisterViewModel model)
        {
            string error = ValidateInput.Validate(model);
            this.GetUserType(req.Session);

            if (error != string.Empty)
            {
                this.AddError(error);

                return this.FileViewResponse(RegisterPath);
            }

            bool result = this.userService.Save(model);

            if (!result)
            {
                this.AddError(ValidationErrors.UsernameTakenError);
                return this.FileViewResponse(RegisterPath);
            }

            //req.Session.Add(SessionStore.CurrentUserKey, model.Email);
            return new RedirectResponse("/login");
        }

        public IHttpResponse Login(IHttpSession session)
        {
            this.GetUserType(session);

            if (session.Contains(SessionStore.CurrentUserKey))
            {
                return new RedirectResponse("/");
            }

            this.ViewData["showError"] = Controller.HideElementClass;

            return this.FileViewResponse(LoginPath);
        }

        public IHttpResponse Login(IHttpRequest req, LoginViewModel model)
        {
            this.GetUserType(req.Session);

            bool result = this.userService.Find(model);

            if (!result)
            {
                this.AddError(ValidationErrors.EmailOrPasswordError);
                return this.FileViewResponse(LoginPath);
            }

            req.Session.Add(SessionStore.CurrentUserKey, model.Email);
            return new RedirectResponse("/");
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            request.Session.Clear();

            return new RedirectResponse("/login");
        }
    }
}
