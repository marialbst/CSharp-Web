namespace HttpServer.Server.HTTP.Response
{
    using Contracts;
    using Enums;
    using System.Text;

    public abstract class HttpResponse : IHttpResponse
    {
        protected HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
        }

        public HttpHeaderCollection Headers { get; }

        public HttpResponseStatusCode StatusCode { get; protected set; }

        private string StatusCodeMessage => this.StatusCode.ToString();

        public override string ToString()
        {
            StringBuilder response = new StringBuilder();

            int statusCode = (int)this.StatusCode;
            response.AppendLine($"HTTP/1.1 {statusCode} {this.StatusCodeMessage}");
            response.AppendLine(this.Headers.ToString());
            response.AppendLine();
            
            return response.ToString();
        }
    }
}
