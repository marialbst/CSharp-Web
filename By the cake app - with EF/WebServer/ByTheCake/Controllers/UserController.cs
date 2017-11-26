namespace WebServer.ByTheCake.Controllers
{
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;
    using Server.HTTP;
    using ViewModels;
    using ViewModels.User;
    using Services;
    using Services.Contracts;
    using System;

    public class UserController : Controller
    {
        private const string RegisterViewPath = @"User\register";
        private const string LoginViewPath = @"User\login";
        private const string ProfileViewPath = @"User\profile";

        private readonly IUserService userService;

        public UserController()
        {
            this.userService = new UserService();
        }

        public IHttpResponse Register()
        {
            this.SetDefaultViewData();

            return this.FileViewResponse(RegisterViewPath);
        }

        public IHttpResponse Register(IHttpRequest req, RegisterUserViewModel model)
        {
            if(model.Username.Length < 3 || model.Password.Length < 3 || model.Password != model.ConfirmPassword)
            {
                this.AddError("Invalid user details");

                return this.FileViewResponse(RegisterViewPath);
            }
            
            if(!this.userService.Create(model.Username, model.Password))
            {
               this.AddError("This username is already taken");

                return this.FileViewResponse(RegisterViewPath);
            }

            this.LoginUser(req, model.Username);

            return new RedirectResponse("/");
        }

        public IHttpResponse Profile(IHttpRequest req)
        {
            if (!req.Session.Contains(SessionStore.CurrentUserKey))
            {
                throw new InvalidOperationException($"There is no logged in user");
            }
            var username = req.Session.Get<string>(SessionStore.CurrentUserKey);

            ProfileViewModel model = this.userService.FindByUsername(username);

            if(model == null)
            {
                throw new InvalidOperationException($"User {username} could not be found");
            }

            this.ViewData["name"] = model.Username;
            this.ViewData["date"] = model.RegisteredOn.ToShortDateString();
            this.ViewData["count"] = model.OrdersCount.ToString();

            return this.FileViewResponse(ProfileViewPath);
        }

        public IHttpResponse Login()
        {
            this.SetDefaultViewData();

            return this.FileViewResponse(LoginViewPath);
        }

        public IHttpResponse Login(IHttpRequest req, LoginUserViewModel model)
        {
            if (string.IsNullOrEmpty(model.Username.Trim())
                || string.IsNullOrEmpty(model.Password.Trim()))
            {
                this.AddError("You have empty fields");

                return this.FileViewResponse(LoginViewPath);
            }

            bool isSuccess=this.userService.Find(model.Username, model.Password);

            if (!isSuccess)
            {
                this.AddError("Wrong password and/or username");

                return this.FileViewResponse(LoginViewPath);
            }

            this.LoginUser(req, model.Username);

            return new RedirectResponse("/");
        }

        private void LoginUser(IHttpRequest req, string name)
        {
            req.Session.Add(SessionStore.CurrentUserKey, name);
            req.Session.Add(Cart.SessionKey, new Cart());
        }

        public IHttpResponse Logout(IHttpRequest req)
        {
            req.Session.Clear();

            return new RedirectResponse("/login");
        }

        private void SetDefaultViewData()
        {
            this.ViewData["authDisplay"] = "none";
        }
    }
}
