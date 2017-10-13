namespace HttpServer.Server.HTTP.Response
{
    using Enums;
    using Common;
    using Exceptions;
    using Server.Contracts;

    public class ViewResponse : HttpResponse
    {
        private readonly IView view;

        public ViewResponse(HttpResponseStatusCode statusCode, IView view)
        {
            CoreValidator.ThrowIfNull(statusCode, nameof(statusCode));
            CoreValidator.ThrowIfNull(view, nameof(view));

            this.ValidateStatusCode(statusCode);

            this.view = view;
            this.StatusCode = statusCode;
        }

        private void ValidateStatusCode(HttpResponseStatusCode statusCode)
        {
            int statusCodeNumber = (int)statusCode;

            if (statusCodeNumber > 299 && statusCodeNumber < 400)
            {
                throw new InvalidResponseException("Status code must be below 300 and above 400");
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}{this.view.View()}";
        }
    }
}
