namespace SimpleMvc.App.Controllers
{
    using Framework.Controllers;
    using Framework.Contracts;
    using Framework.Attributes.Methods;
    using Models;
    using Data.Services.Contracts;
    using Data.Services;
    using WebServer.Exceptions;
    using SimpleMvc.Domain.Entities;
    using System.Linq;
    using SimpleMvc.Framework.ActionResults;
    using System.Collections.Generic;
    using System;

    public class UsersController : Controller
    {
        private readonly IUserService userService;

        public UsersController()
        {
            this.userService = new UserService();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUserModel model)
        {
            if (!this.IsValidModel(model))
            {
                return View();
            }

            this.userService.AddUserToTheDb(model.Username, model.Password);

            return new RedirectResult("/home/index");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Login(LoginUserModel model)
        {
            bool fooundUser = this.userService.FindUser(model.Username, model.Password);

            if (!fooundUser)
            {
                return new RedirectResult("/home/login");
            }

            this.SignIn(model.Username);

            return new RedirectResult("/home/index");
        }

        [HttpGet]
        public IActionResult All()
        {
            if (!this.User.IsAuthenticated)
            {
                return new RedirectResult("/users/login");
            }

            IDictionary<int, string> users = this.userService.AllUsers();

            this.Model["users"] = users.Any() ?
                string.Join(
                    Environment.NewLine,
                    users.Select(u => $"<li><a href=\"/users/profile?id={u.Key}\">{u.Value}</a></li>")) 
                :string.Empty;

            return View();
        }

        [HttpGet]
        public IActionResult Profile(int id)
        {
            if (!this.User.IsAuthenticated)
            {
                return new RedirectResult("/users/login");
            }

            User user = this.userService.GetUserById(id);

            if (user == null)
            {
                throw new InvalidResponseException("User doesn't exist");
            }

            if(user.Username != this.User.Name)
            {
                this.Model["sameUser"] = "none";
            }

            this.Model["username"] = user.Username;
            this.Model["userId"] = user.Id.ToString();
            this.Model["notes"] = user.Notes.Any()  ?
                string.Join(
                    Environment.NewLine,
                    user.Notes.Select(n => $"<li><strong>{n.Title}</strong> - {n.Content}</li>"))
                : string.Empty;
            this.Model["sameUser"] = "block";

            return View();
        }

        [HttpPost]
        public IActionResult Profile(AddNoteModel model)
        {

            this.userService.AddNoteToUser(model.UserId, model.Title, model.Content);

            return this.Profile(model.UserId);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            this.SignOut();

            return new RedirectResult("/home/index");
        }
    }
}
