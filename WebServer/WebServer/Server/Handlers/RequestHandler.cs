namespace WebServer.Server.Handlers
{
    using System;
    using Common;
    using Contracts;
    using HTTP.Contracts;

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
            IHttpResponse httpResponse = this.handlingFunc.Invoke(httpContext);

            httpResponse.AddHeader("Content-Type", "text/html");

            return httpResponse;           
        }
    }
}
