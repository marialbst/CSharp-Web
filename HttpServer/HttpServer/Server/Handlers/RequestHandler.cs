namespace HttpServer.Server.Handlers
{
    using System;
    using Contracts;
    using Common;
    using HTTP;
    using HTTP.Contracts;

    public abstract class RequestHandler : IRequestHandler
    {
        private readonly Func<IHttpContext, IHttpResponse> handlingFunc;

        protected RequestHandler(Func<IHttpContext, IHttpResponse> handlingFunc)
        {
            CoreValidator.ThrowIfNull(handlingFunc, nameof(handlingFunc));

            this.handlingFunc = handlingFunc;
        }

        public IHttpResponse Handle(IHttpContext context)
        {
            IHttpResponse response = this.handlingFunc.Invoke(context);

            response.Headers.Add(new HttpHeader("Content-Type", "text/plain"));

            return response;
        }
    }
}
