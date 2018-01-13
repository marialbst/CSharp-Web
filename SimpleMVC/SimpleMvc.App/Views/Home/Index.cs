namespace SimpleMvc.App.Views.Home
{
    using System.Text;
    using Framework.Contracts;

    public class Index : IRenderable
    {
        public string Render()
        {
            StringBuilder html = new StringBuilder();

            html.AppendLine("<h2>Notes App</h2>");
            html.AppendLine("<a href=\"/users/all\">All users</a>");
            html.AppendLine("<a href=\"/users/register\">Register user</a>");

            return html.ToString();
        }
    }
}
