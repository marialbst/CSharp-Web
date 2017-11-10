namespace WebServer.ByTheCakeApp.Views
{
    using System.IO;
    using Server.Contracts;

    public class LoginnView : IView
    {
        private const string DefaultPath = @"ByTheCakeApp\Resources\Home\login.html";
        private const string ResultPlaceholder = "{{{result}}}";
        
        private string username;
        private string password;

        public LoginnView()
        {

        }

        public LoginnView(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string View()
        {
            string html = File.ReadAllText(DefaultPath);
            string replacement = "";

            if (this.username != null && this.password != null)
            {
               replacement = $"<span class=\"red\">Invalid password or username</span>";
            }
             
            
            string result = html.Replace(ResultPlaceholder, replacement);

            return result;
        }
    }
}
