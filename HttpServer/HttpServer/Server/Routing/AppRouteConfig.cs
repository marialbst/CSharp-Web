namespace HttpServer.Server.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Enums;
    using Handlers;
    using HttpServer.Server.HTTP.Contracts;

    public class AppRouteConfig : IAppRouteConfig
    {
        private readonly IDictionary<HttpRequestMethod, IDictionary<string, RequestHandler>> routes;

        public AppRouteConfig()
        {
            this.routes = new Dictionary<HttpRequestMethod, IDictionary<string, RequestHandler>>();

            var methods = Enum
                .GetValues(typeof(HttpRequestMethod))
                .Cast<HttpRequestMethod>();

            foreach (HttpRequestMethod method in methods)
            {
                this.routes[method] = new Dictionary<string, RequestHandler>();
            }
        }

        public IDictionary<HttpRequestMethod, IDictionary<string, RequestHandler>> Routes { get { return this.routes; } }

        public void Get(string route, Func<IHttpContext, IHttpResponse> handler)
        {
            this.AddRoute(route, new GetHandler(handler));
        }

        public void Post(string route, Func<IHttpContext, IHttpResponse> handler)
        {
            this.AddRoute(route, new PostHandler(handler));
        }


        public void AddRoute(string route, RequestHandler handler)
        {
            var handlerName = handler.GetType().Name.ToLower();

            if (handlerName.StartsWith("get"))
            {
                this.routes[HttpRequestMethod.Get].Add(route,handler);
            }
            else if (handlerName.StartsWith("post"))
            {
                this.routes[HttpRequestMethod.Post].Add(route, handler);
            }
            else
            {
                throw new InvalidOperationException("Invalid handler.");
            }
        }
    }
}
