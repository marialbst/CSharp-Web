namespace WebServer.Server.Routing
{
    using System;
    using HTTP.Contracts;
    using System.Linq;
    using Contracts;
    using System.Collections.Generic;
    using Enums;
    using Handlers;

    public class AppRouteConfig : IAppRouteConfig
    {
        private readonly Dictionary<HttpRequestMethod, Dictionary<string, RequestHandler>> routes;

        public AppRouteConfig()
        {
            this.routes = new Dictionary<HttpRequestMethod, Dictionary<string, RequestHandler>>();

            var methods = Enum.GetValues(typeof(HttpRequestMethod)).Cast<HttpRequestMethod>();

            foreach (var method in methods)
            {
                this.routes.Add(method, new Dictionary<string, RequestHandler>());
            }
        }

        public IReadOnlyDictionary<HttpRequestMethod, Dictionary<string, RequestHandler>> Routes { get { return this.routes; } }

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
            string method = handler.GetType().ToString().ToLower();

            if (method.Contains("get"))
            {
                this.Routes[HttpRequestMethod.GET].Add(route, handler);
            }
            else if(method.Contains("post"))
            {
                this.Routes[HttpRequestMethod.POST].Add(route, handler);
            }
            else
            {
                throw new InvalidOperationException("Invalid handler.");
            }
        }
    }
}
