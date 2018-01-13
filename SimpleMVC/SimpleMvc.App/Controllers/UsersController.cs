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

        public IActionResult<AllUsersViewModel> All()
        {
            AllUsersViewModel viewModel = new AllUsersViewModel()
            {
                Users = this.userService.AllUsers()
            };

            return this.View(viewModel);
        }

        [HttpGet]
        public IActionResult<UserProfileViewModel> Profile(int id)
        {
            User user = this.userService.GetUserById(id);

            if (user == null)
            {
                throw new InvalidResponseException("User doesn't exist");
            }

            UserProfileViewModel viewModel = new UserProfileViewModel
            {
                UserId = user.Id,
                Username = user.Username,
                Notes = user.Notes.Select(n => new NoteViewModel
                {
                    Title = n.Title,
                    Content = n.Content
                })
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult<UserProfileViewModel> Profile(AddNoteModel model)
        {
            this.userService.AddNoteToUser(model.UserId, model.Title, model.Content);

            return this.Profile(model.UserId);
        }
    }
}
