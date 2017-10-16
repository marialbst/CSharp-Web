namespace WebServer.Server.HTTP.Response
{
    using Server.Contracts;
    using Common;
    using Enums;
    using Exceptions;
    using System.Text;

    public class ViewResponse : HttpResponse
    {
        private readonly IView view;

        public ViewResponse(HttpStatusCode statusCode, IView view)
        {
            CoreValidator.ThrowIfNull(statusCode, nameof(statusCode));
            CoreValidator.ThrowIfNull(view, nameof(view));

            this.ValidateStatusCode(statusCode);

            this.StatusCode = statusCode;
            this.view = view;
        }

        public override string Response
        {
            get
            {
                StringBuilder response = new StringBuilder();

                response.AppendLine($"HTTP/1.1 {(int)this.StatusCode} {this.StatusMessage}");

                response.AppendLine(this.Headers.ToString());
                response.AppendLine();

                if ((int)this.StatusCode < 300 | (int)this.StatusCode > 400)
                {
                    response.AppendLine(this.view.View());
                }

                return response.ToString();
            }
        }

        private void ValidateStatusCode(HttpStatusCode statusCode)
        {
            int statusCodeNumber = (int)statusCode;

            if (statusCodeNumber > 299 && statusCodeNumber < 400)
            {
                throw new InvalidResponseException("Status code must be below 300 and above 400");
            }
        }
    }
}
