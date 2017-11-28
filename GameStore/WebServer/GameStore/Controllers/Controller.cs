namespace WebServer.GameStore.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Server.Enums;
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;
    using Server;
    using Enums;
    using System;
    using Server.HTTP;
    using Service.Contracts;
    using Service;

    public abstract class Controller
    {
        public const string DefaultPath = @"GameStore\Resourses\{0}.html";
        public const string ContentPlaceholder = "{{{content}}}";
        public const string HideElementClass = "hideElement";

        private IUserService userService;

        public Controller()
        {
            this.userService = new UserService();

            this.ViewData = new Dictionary<string, string>
            {
                ["user"] = HideElementClass,
                ["guest"] = HideElementClass,
                ["admin"] = HideElementClass
            };
        }

        protected IDictionary<string, string> ViewData { get; private set; }

        public IHttpResponse FileViewResponse(string fileName)
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

        private string ProcessFileHtml(string fileName)
        {
            var layoutHtml = File.ReadAllText(string.Format(DefaultPath, "layout"));

            var fileHtml = File
                .ReadAllText(string.Format(DefaultPath, fileName));

            var result = layoutHtml.Replace(ContentPlaceholder, fileHtml);

            return result;
        }

        protected void GetUserType(IHttpSession session)
        {
            string userKey = SessionStore.CurrentUserKey;

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

            this.ViewData[parsedName] = string.Empty;
        }

        protected void AddError(string errorMsg)
        {
            this.ViewData["showError"] = string.Empty;
            this.ViewData["error"] = errorMsg;
        }
    }

}
