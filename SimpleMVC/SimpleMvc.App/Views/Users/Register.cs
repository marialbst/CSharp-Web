namespace SimpleMvc.App.Views.Users
{
    using System.Text;
    using Framework.Contracts;

    public class Register : IRenderable
    {
        public string Render()
        {
            StringBuilder html = new StringBuilder();

            html.AppendLine($"<h2>Register new user:</h2>");
            html.AppendLine($"<form action=\"register\" method=\"POST\">");

            html.AppendLine("Username: <input type=\"text\" name=\"Username\" /><br />");
            html.AppendLine("Password: <input type=\"password\" name=\"Password\" /><br />");
            html.AppendLine("<input type=\"submit\" value=\"Register\" /><br />");

            html.AppendLine($"</form>");
            return html.ToString();
        }
    }
}
