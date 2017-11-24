namespace WebServer.ByTheCake.Controllers
{
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;
    using Server.HTTP;
    using ViewModels;
    using ViewModels.User;
    using Services;
    using Services.Interfaces;
    using System;

    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController()
        {
            this.userService = new UserService();
        }

        public IHttpResponse Register()
        {
            this.SetDefaultViewData();

            return this.FileViewResponse(@"User\register");
        }

        public IHttpResponse Register(IHttpRequest req, RegisterUserViewModel model)
        {
            if(model.Username.Length < 3 || model.Password.Length < 3 || model.Password != model.ConfirmPassword)
            {
                this.ViewData["showError"] = "block red";

                if (model.Username.Length < 3)
                {
                    this.ViewData["error"] += "Username have to be atleast 3 symbols<br />";
                }

                if (model.Password.Length < 3)
                {
                    this.ViewData["error"] += "Password have to be atleast 3 symbols<br />";
                }

                if (model.Password != model.ConfirmPassword)
                {
                    this.ViewData["error"] += "Passwords doesn't match<br />";
                }
                return this.FileViewResponse(@"User\register");
            }
            
            if(!this.userService.Create(model.Username, model.Password))
            {
                this.ViewData["showError"] = "block red";
                this.ViewData["error"] = "This username is already taken<br />";

                return this.FileViewResponse(@"User\register");
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

            return this.FileViewResponse(@"User\profile");
        }

        public IHttpResponse Login()
        {
            this.SetDefaultViewData();

            return this.FileViewResponse(@"User\login");
        }

        public IHttpResponse Login(IHttpRequest req, LoginUserViewModel model)
        {
            if (string.IsNullOrEmpty(model.Username.Trim())
                || string.IsNullOrEmpty(model.Password.Trim()))
            {
                this.ViewData["error"] = "You have empty fields";
                this.ViewData["showError"] = "block red";

                return this.FileViewResponse(@"User\login");
            }

            bool isSuccess=this.userService.Find(model.Username, model.Password);

            if (!isSuccess)
            {
                this.ViewData["error"] = "Wrong password and/or username";
                this.ViewData["showError"] = "block red";

                return this.FileViewResponse(@"User\login");
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
