namespace WebServer.ByTheCakeApp.Controllers
{
    using Server.HTTP.Contracts;
    using Server.Enums;
    using Helpers;
    using Server.HTTP.Response;
    using Views;
    using System;
    using System.Collections.Generic;

    public class HomeController : Controller
    {
        private const string Username = "suAdmin";
        private const string Password = "aDmInPw17";

        public IHttpResponse Index()
        {
            return this.FileViewResponse(@"Home\index");
        }

        public IHttpResponse About()
        {
            return this.FileViewResponse(@"Home\about");
        }

        public IHttpResponse Login()
        {
            return new ViewResponse(HttpStatusCode.Ok, new LoginnView());
        }

        public IHttpResponse Login(Dictionary<string, string> data)
        {
            string username = "";
            string password = "";

            if (data.ContainsKey("username"))
            {
                username = data["username"];
            }

            if (data.ContainsKey("password"))
            {
                password = data["password"];
            }

            if (username != Username || password != Password)
            {
                return new ViewResponse(HttpStatusCode.Ok, new LoginnView(username, password));
            }
   
            return new RedirectResponse("/email");
        }
    }
}
