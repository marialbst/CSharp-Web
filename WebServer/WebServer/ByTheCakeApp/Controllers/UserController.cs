namespace WebServer.ByTheCakeApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Server.Enums;
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;
    using Views;

    public class UserController
    {
        public IHttpResponse LoginGet()
        {
            return new ViewResponse(HttpStatusCode.Ok, new LoginView());
        }

        public IHttpResponse LoginPost(string name, string password)
        {
            return new ViewResponse(HttpStatusCode.Ok, new LoginView(name, password));
        }

        public IHttpResponse EmailGet()
        {
            return new ViewResponse(HttpStatusCode.Ok, new EmailView());
        }

        public IHttpResponse EmailPost(Dictionary<string, string> data)
        {
            string email = "";
            string subject = "";
            string message = "";

            if (data.ContainsKey("to"))
            {
                email = data["to"];
            }

            if (data.ContainsKey("subj"))
            {
                subject = data["subj"];
            }

            if (data.ContainsKey("msg"))
            {
                message = data["msg"];
            }

            if (this.IsValid(email, subject) && subject.Length != 0 && message.Length != 0)
            {
                return new RedirectResponse("/success");
            }

            return new ViewResponse(HttpStatusCode.Ok, new EmailView(email, subject, message));
        }

        public IHttpResponse Success()
        {
            return new ViewResponse(HttpStatusCode.Ok, new OkView());
        }

        private bool IsValid(string email, string subject)
        {
            Regex rgx = new Regex(@"^([a-zA-Z0-9]+[.-_]?[a-zA-Z0-9]+)(@)[a-zA-Z0-9]+(\.[a-zA-Z0-9]{2,})+$");

            if (!rgx.IsMatch(email))
            {
                return false;
            }
            if (subject.Length > 100)
            {
                return false;
            }

            return true;
        }
    }
}
