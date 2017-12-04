namespace WebServer.GameStore.Controllers
{
    using Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using Server;
    using Server.Enums;
    using Server.HTTP;
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;
    using Service;
    using Service.Contracts;
    using ViewModels;

    public abstract class Controller
    {
        public const string DefaultPath = @"GameStore\Resourses\{0}.html";
        public const string ContentPlaceholder = "{{{content}}}";
        public const string AddHideElementClass = "hideElement";
        public const string RemoveHideElementClass = "";

        private readonly IUserService userService;
        
        public Controller(IHttpRequest request)
        {
            this.userService = new UserService();
            this.Request = request;

            if (!this.Request.Session.Contains(SessionStore.ShoppingCartKey))
            {
                this.Request.Session.Add(SessionStore.ShoppingCartKey, new Cart());
            }

            this.ViewData = new Dictionary<string, string>
            {
                ["showError"] = AddHideElementClass,
                ["user"] = AddHideElementClass,
                ["guest"] = AddHideElementClass,
                ["admin"] = AddHideElementClass,
                ["itemsCount"] = $"{this.ItemsCount} {(this.ItemsCount != 1 ? "items":"item")}"
            };

            this.GetUserType();
        }

        protected IHttpRequest Request { get; private set; }

        protected int ItemsCount
            => this.Request.Session.Get<Cart>(SessionStore.ShoppingCartKey).Games.Count;

        protected bool IsAuthenticated
        {
            get
            {
                return this.Request.Session.Contains(SessionStore.CurrentUserKey);
            }
        }

        protected bool IsAdmin
        {
            get
            {
                string currentUser = this.Request.Session.Get<string>(SessionStore.CurrentUserKey);

                return this.userService.IsAdmin(currentUser);
            }
        }

        protected IDictionary<string, string> ViewData { get; private set; }

        protected IHttpResponse FileViewResponse(string fileName)
        {
            var result = this.ProcessFileHtml(fileName);

            if (this.ViewData.Any())
            {
                foreach (var value in this.ViewData)
                {
                    result = result.Replace($"{{{{{{{value.Key}}}}}}}", value.Value);
                }
            }

            return new ViewResponse(HttpStatusCode.Ok, new FileView(result));
        }

        protected bool ValidateModel(object model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(model, context, results, true))
            {
                foreach (var result in results)
                {
                    if(result != ValidationResult.Success)
                    {
                        this.AddError(result.ErrorMessage);

                        return false;
                    }
                }
            }

            return true;
        }

        private string ProcessFileHtml(string fileName)
        {
            string layoutHtml = File.ReadAllText(string.Format(DefaultPath, "layout"));

            string fileHtml = File
                .ReadAllText(string.Format(DefaultPath, fileName));

            string result = layoutHtml.Replace(ContentPlaceholder, fileHtml);

            return result;
        }

        private void GetUserType()
        {
            string userKey = SessionStore.CurrentUserKey;
            var session = this.Request.Session;

            if (!session.Contains(userKey))
            {
                this.ShowNavbar(UserType.Guest);
            }
            else
            {
                string user = session.Get<string>(userKey);

                if (this.userService.IsAdmin(user))
                {
                    this.ShowNavbar(UserType.Admin);
                }

                this.ShowNavbar(UserType.User);
            }
        }

        private void ShowNavbar(UserType visitorType)
        {
            string parsedName = Enum.GetName(typeof(UserType), visitorType).ToLower();

            this.ViewData.Remove(parsedName);
        }

        protected void AddError(string errorMsg)
        {
            this.ViewData["showError"] = RemoveHideElementClass;
            this.ViewData["error"] = errorMsg;
        }
    }

}
