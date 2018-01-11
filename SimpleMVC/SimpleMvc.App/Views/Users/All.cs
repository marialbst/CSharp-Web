namespace SimpleMvc.App.Views.Users
{
    using System.Text;
    using Framework.Contracts.Generic;
    using ViewModels;

    public class All : IRenderable<AllUsernamesViewModel>
    {
        public AllUsernamesViewModel Model { get; set; }

        public string Render()
        {
            StringBuilder html = new StringBuilder();
            var usernames = Model.Usernames;
            
            html.AppendLine("<h2>All users:</h2>");
            html.AppendLine("<ul>");

            foreach (var username in usernames)
            {
                html.AppendLine($"<li>{username}</li>");
            }

            html.AppendLine("</ul>");

            return html.ToString();
        }
    }
}
