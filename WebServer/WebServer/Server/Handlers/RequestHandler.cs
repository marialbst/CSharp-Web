namespace WebServer.Server.Handlers
{
    using System;
    using Common;
    using Contracts;
    using HTTP.Contracts;
    using HTTP;

    public abstract class RequestHandler : IRequestHandler
    {
        private readonly Func<IHttpContext, IHttpResponse> handlingFunc;

        public RequestHandler(Func<IHttpContext, IHttpResponse> handlingFunc)
        {
            CoreValidator.ThrowIfNull(handlingFunc, nameof(handlingFunc));

            this.handlingFunc = handlingFunc;
        }

        public IHttpResponse Handle(IHttpContext httpContext)
        {
            string sessionIdToSend = null;

            if (!httpContext.Request.Cookies.ContainsKey(SessionStore.SessionCookieKey))
            {
                string sessionId = Guid.NewGuid().ToString();

                httpContext.Request.Session = SessionStore.Get(sessionId);

                sessionIdToSend = sessionId;
            }   

            IHttpResponse httpResponse = this.handlingFunc.Invoke(httpContext);

            if (sessionIdToSend != null)
            {
                httpResponse.Headers.Add(
                        HttpHeader.SetCookie,
                        $"{SessionStore.SessionCookieKey}={sessionIdToSend}; HttpOnly; Path=/");
            }

            if (!httpResponse.Headers.ContainsKey(HttpHeader.ContentType))
            {
                httpResponse.AddHeader(HttpHeader.ContentType, "text/html");
            }

            foreach (var cookie in httpResponse.Cookies)
            {
                httpResponse.AddHeader(HttpHeader.Cookie, cookie.ToString());
            }
            return httpResponse;           
        }
    }
}
