namespace WebServer.ByTheCake.Controllers
{
    using Views;
    using Server.Enums;
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System;
    using System.Net.Mail;
    using System.Text;

    public class UserController
    {
        private const string DefinedUsername = "suAdmin";
        private const string DefinedPassword = "aDmInPw17";

        //Get and Post for task 15
        public IHttpResponse Loginn()
        {
            return new ViewResponse(HttpStatusCode.Ok, new LoginnView());
        }

        public IHttpResponse Loginn(Dictionary<string, string> formData)
        {
            string name = string.Empty;
            string password = string.Empty;

            if (formData.ContainsKey("username") && formData.ContainsKey("password"))
            {
                name = formData["username"];
                password = formData["password"];
            }

            return new ViewResponse(HttpStatusCode.Ok, new LoginnView(name, password));
        }

        //Get and Post for task 16
        public IHttpResponse Login()
        {
            return new ViewResponse(HttpStatusCode.Ok, new LoginView());
        }

        public IHttpResponse Login(Dictionary<string, string> formData)
        {
            string name = string.Empty;
            string password = string.Empty;

            if (formData.ContainsKey("username") && formData.ContainsKey("password"))
            {
                name = formData["username"];
                password = formData["password"];
            }

            if (name != DefinedUsername || password != DefinedPassword)
            {
                return new ViewResponse(HttpStatusCode.Ok, new LoginView(true));
            }

            return new RedirectResponse("/email");
        }

        public IHttpResponse Email()
        {
            return new ViewResponse(HttpStatusCode.Ok, new EmailView());
        }

        public IHttpResponse Email(Dictionary<string, string> data)
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
                //will throw an exeption if you try sendingi mail without correct mail&pass
                //this.SendEmail(email, subject, message);
                return new RedirectResponse("/");
            }

            return new ViewResponse(HttpStatusCode.Ok, new EmailView(email, subject, message));
        }

        public IHttpResponse Greeting(IHttpSession session)
        {
            return new ViewResponse(HttpStatusCode.Ok, new GreetingView(session));
        }

        public IHttpResponse Greeting(IHttpRequest request)
        {
            IHttpSession session = request.Session;
            Dictionary<string, string> formData = request.FormData;

            if (session.Get("firstName") == null)
            {
                session.Add("firstName", formData["firstName"]);
            }
            else if (session.Get("lastName") == null)
            {
                session.Add("lastName", formData["lastName"]);
            }
            else if (session.Get("age") == null)
            {
                session.Add("age", formData["age"]);
            }
            return new RedirectResponse("/greeting");
        }

        private void SendEmail(string recipient, string subject, string message)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            //will work if you replace with own gmail mail and password
            client.Credentials = new System.Net.NetworkCredential("someemail@gmail.com", "passForIt");
            MailMessage mm = new MailMessage("someemail@gmail.com", recipient, subject, message);

            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);
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
