namespace WebServer.ByTheCakeApp.Helpers
{
    using System.Collections.Generic;
    using System.IO;
    using Views;
    using Server.Enums;
    using Server.HTTP.Response;
    using Server.HTTP.Contracts;
    using System.Text;
    using WebServer.ByTheCakeApp.Models;

    public abstract class Controller
    {
        private const string DefaultPath = @"ByTheCakeApp\Resources\{0}.html";
        private const string ContentPlaceholder = "{{{content}}}";
        private const string ResultPlaceholder = "{{{result}}}";
        private const string UserPlaceholder = "{{{user}}}";

        public IHttpResponse FileViewResponse(string fileName, List<Cake> cakes = null)
        {
            string layoutHtml = File.ReadAllText(string.Format(DefaultPath, "layout"));
            string fileHtml = File.ReadAllText(string.Format(DefaultPath, fileName));

            string html = layoutHtml.Replace(ContentPlaceholder, fileHtml);

            string replacement = this.FormatReplacement(cakes); 

            string resultHtml = html.Replace(ResultPlaceholder, replacement);

            return new ViewResponse(HttpStatusCode.Ok, new FileView(resultHtml));
        }

        public IHttpResponse FileViewResponse(string fileName, Dictionary<string, string> result)
        {
            string layoutHtml = File.ReadAllText(string.Format(DefaultPath, "layout"));
            string fileHtml = File.ReadAllText(string.Format(DefaultPath, fileName));

            string html = layoutHtml.Replace(ContentPlaceholder, fileHtml);

            string replacement = this.FormatReplacement(result);

            string resultHtml = html.Replace(ResultPlaceholder, replacement);

            return new ViewResponse(HttpStatusCode.Ok, new FileView(resultHtml));
        }

        private string FormatReplacement(List<Cake> cakes)
        {
            if (cakes != null)
            {
                StringBuilder replacement = new StringBuilder();

                foreach (var cake in cakes)
                {
                    replacement.AppendLine($"<div>{cake.Name}: ${cake.Price} <a href=\"/order?id={cake.Id}\">Order</a></div>");
                }

                return replacement.ToString();
            }

            return string.Empty;
        }

        private string FormatReplacement(Dictionary<string, string> result)
        {

            StringBuilder replacement = new StringBuilder();

            foreach (var item in result)
            {
                replacement.AppendLine($"<div>{item.Key}: {item.Value}</div>");
            }

            return replacement.ToString();
        }
    }
}
