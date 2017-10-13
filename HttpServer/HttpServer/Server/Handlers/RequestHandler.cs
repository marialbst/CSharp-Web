﻿namespace HttpServer.Server.Handlers
{
    using System;
    using Contracts;
    using Common;
    using HTTP;
    using HTTP.Contracts;

    public abstract class RequestHandler : IRequestHandler
    {
        private readonly Func<IHttpRequest, IHttpResponse> handlingFunc;

        protected RequestHandler(Func<IHttpRequest, IHttpResponse> handlingFunc)
        {
            CoreValidator.ThrowIfNull(handlingFunc, nameof(handlingFunc));

            this.handlingFunc = handlingFunc;
        }

        public IHttpResponse Handle(IHttpContext context)
        {
            IHttpResponse response = this.handlingFunc(context.Request);

            response.Headers.Add(new HttpHeader("Content-Type", "text/plain"));

            return response;
        }
    }
}
