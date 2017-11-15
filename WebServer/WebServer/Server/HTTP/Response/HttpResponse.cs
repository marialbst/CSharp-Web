namespace WebServer.Server.HTTP.Response
{
    using Contracts;
    using Enums;

    public abstract class HttpResponse : IHttpResponse
    {
        protected HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();
        }

        public IHttpHeaderCollection Headers { get; private set; }

        public IHttpCookieCollection Cookies { get; private set; }

        public HttpStatusCode StatusCode { get; protected set; }

        public string StatusMessage { get { return this.StatusCode.ToString(); } }

        public abstract string Response { get; }

        public byte[] Data { get; private set; }

        public void AddHeader(string key, string value)
        {
            this.Headers.Add(new HttpHeader(key, value));
        }
    }
}
