namespace HttpServer.Server.Routing.Contracts
{
    using System.Collections.Generic;
    using Enums;
    using Handlers;
    using HTTP.Contracts;
    using System;

    public interface IAppRouteConfig
    {
        IDictionary<HttpRequestMethod, IDictionary<string, RequestHandler>> Routes { get; }

        void Get(string route, Func<IHttpContext, IHttpResponse> handler);

        void Post(string route, Func<IHttpContext, IHttpResponse> handler);

        void AddRoute(string route, RequestHandler handler);
    }
}
