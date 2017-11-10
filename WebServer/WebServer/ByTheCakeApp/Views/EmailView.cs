namespace WebServer.ByTheCakeApp.Views
{
    using Server.Contracts;
    using System.IO;
    using Controllers;

    public class EmailView : IView
    {
        private const string DefaultPath = @"ByTheCakeApp\Resources\Home\email.html";
        private const string ResultPlaceholder = "{{{result}}}";

        private string email;
        private string subject;
        private string message;

        public EmailView()
        {

        }

        public EmailView(string email, string subj, string msg)
        {
            this.email = email;
            this.subject = subj;
            this.message = msg;
        }

        public string View()
        {
            string html = File.ReadAllText(DefaultPath);

            string replacement = "";

            if(this.email != null || this.message != null || this.subject != null)
            {
                replacement = "<ul>";

                if (string.IsNullOrWhiteSpace(this.email))
                {
                    replacement += $"<li class=\"red\">Invalid email</li>";
                }

                if (string.IsNullOrWhiteSpace(this.subject))
                {
                    replacement += $"<li class=\"red\">Invalid subject</li>";
                }

                if (string.IsNullOrWhiteSpace(this.message))
                {
                    replacement += $"<li class=\"red\">Invalid message</li>";
                }

                replacement += "</ul>";
            }
            
            string result = html.Replace(ResultPlaceholder, replacement);

            return result;
        }
    }
}
