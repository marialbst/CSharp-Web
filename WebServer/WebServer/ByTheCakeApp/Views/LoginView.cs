namespace WebServer.ByTheCakeApp.Views
{
    using System.IO;
    using Server.Contracts;

    public class LoginView : IView
    {
        private const string DefaultPath = @"ByTheCakeApp\Resources\Home\login.html";
        private const string ResultPlaceholder = "{{{result}}}";

        private string username;
        private string password;

        public LoginView(string username = "", string password = "")
        {
            this.username = username;
            this.password = password;
        }

        public string View()
        {
            string html = File.ReadAllText(DefaultPath);
            string replacement = "";

            if (this.username != string.Empty && this.password != string.Empty)
            {
                replacement = $"Hi {this.username}, your password is: {this.password}";
            }

            string result = html.Replace(ResultPlaceholder, replacement);

            return result;
        }
    }
}
