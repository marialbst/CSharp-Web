namespace SimpleMvc.App.Controllers
{
    using Framework.Controllers;
    using Framework.Contracts;
    using Framework.Attributes.Methods;
    using Models;
    using Data.Services.Contracts;
    using Data.Services;
    using System;
    using Framework.Contracts.Generic;
    using App.ViewModels;

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
            bool result = this.userService.AddUserToTheDb(model.Username, model.Password);

            //if (result)
            //{
            //    Console.WriteLine("User registered!");
            //}
            //else
            //{
            //    Console.WriteLine("User already exists!");
            //}

            return this.Register();
        }

        public IActionResult<AllUsernamesViewModel> All()
        {
            AllUsernamesViewModel viewModel = new AllUsernamesViewModel()
            {
                Usernames = this.userService.AllUsernames()
            };

            return this.View(viewModel);
        }
    }
}
