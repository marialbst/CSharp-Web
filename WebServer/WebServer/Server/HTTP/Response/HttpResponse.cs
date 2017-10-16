﻿namespace WebServer.Server.HTTP.Response
{
    using Contracts;
    using Enums;

    public abstract class HttpResponse : IHttpResponse
    {
        protected HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
        }

        public IHttpHeaderCollection Headers { get; private set; }

        public HttpStatusCode StatusCode { get; protected set; }

        public string StatusMessage { get { return this.StatusCode.ToString(); } }

        public abstract string Response { get; }

        public void AddHeader(string key, string value)
        {
            this.Headers.Add(new HttpHeader(key, value));
        }
    }
}