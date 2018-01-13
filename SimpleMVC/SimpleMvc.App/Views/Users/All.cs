namespace SimpleMvc.App.Views.Users
{
    using System.Text;
    using Framework.Contracts.Generic;
    using Helpers;
    using ViewModels;

    public class All : IRenderable<AllUsersViewModel>
    {
        public AllUsersViewModel Model { get; set; }

        public string Render()
        {
            StringBuilder html = new StringBuilder();
            var users = Model.Users;

            html.AppendLine(Constants.BackHomeConstant);

            html.AppendLine("<h2>All users:</h2>");
            html.AppendLine("<ul>");

            foreach (var user in users)
            {
                html.AppendLine($"<li><a href=\"/users/profile?id={user.Key}\">{user.Value}</li>");
            }

            html.AppendLine("</ul>");

            return html.ToString();
        }
    }
}
