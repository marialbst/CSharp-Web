namespace WebServer.ByTheCake.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Views;
    using Server.Enums;
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;

    public abstract class Controller
    {
        public const string DefaultPath = @"ByTheCake\Resources\{0}.html";
        public const string ContentPlaceholder = "{{{content}}}";

        public Controller()
        {
            this.ViewData = new Dictionary<string, string>
            {
                ["showError"] = "none",
                ["authDisplay"] = "block"
            };
        }

        protected IDictionary<string, string> ViewData { get; private set; }

        public IHttpResponse FileViewResponse(string fileName)
        {
            var result = this.ProcessFileHtml(fileName);

            if (this.ViewData.Any())
            {
                foreach (var value in this.ViewData)
                {
                    result = result.Replace($"{{{{{{{value.Key}}}}}}}", value.Value);
                }
            }
            
            return new ViewResponse(HttpStatusCode.Ok, new FileView(result));
        }

        private string ProcessFileHtml(string fileName)
        {
            var layoutHtml = File.ReadAllText(string.Format(DefaultPath, "layout"));

            var fileHtml = File
                .ReadAllText(string.Format(DefaultPath, fileName));

            var result = layoutHtml.Replace(ContentPlaceholder, fileHtml);

            return result;
        }

        protected void AddError(string errorMsg)
        {
            this.ViewData["showError"] = "block red";
            this.ViewData["error"] = errorMsg;
        }
    }
}
