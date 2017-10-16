namespace WebServer.Server.HTTP.Response
{
    using Common;
    using Enums;
    using System.Text;

    public class RedirectResponse : HttpResponse
    {
        public RedirectResponse(string redirectUrl)
        {
            CoreValidator.ThrowIfNullOrEmpty(redirectUrl, nameof(redirectUrl));
            this.AddHeader("Location", redirectUrl);
            this.StatusCode = HttpStatusCode.Found;    
        }

        public override string Response
        {
            get
            {
                StringBuilder response = new StringBuilder();

                response.AppendLine($"HTTP/1.1 {(int)this.StatusCode} {this.StatusMessage}");

                response.AppendLine(this.Headers.ToString());

                response.AppendLine();

                return response.ToString();
            }
        }
    }
}
