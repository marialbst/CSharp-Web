namespace SimpleMvc.App.Views.Users
{
    using System.Text;
    using App.ViewModels;
    using Framework.Contracts.Generic;

    public class Profile : IRenderable<UserProfileViewModel>
    {
        public UserProfileViewModel Model { get; set; }

        public string Render()
        {
            StringBuilder html = new StringBuilder();

            html.AppendLine($"<h2>User: {Model.Username}</h2>");

            html.AppendLine($"<form action=\"profile\" method=\"POST\">");
            html.AppendLine($"Title: <input type=\"text\" name=\"Title\" /><br />");
            html.AppendLine($"Content: <input type=\"text\" name=\"Content\" /><br />");
            html.AppendLine($"<input type=\"hidden\" name=\"UserId\" value=\"{Model.UserId}\" />");
            html.AppendLine($"<input type=\"submit\" value=\"Add note\" /><br />");
            html.AppendLine($"</form>");

            html.AppendLine("<h5>List of Notes</h5>");
            html.AppendLine("<ul>");

            foreach (var note in Model.Notes)
            {
                html.AppendLine($"<li><strong>{note.Title}</strong> - {note.Content}</li>");
            }

            html.AppendLine("</ul>");
            return html.ToString();
        }
    }
}
