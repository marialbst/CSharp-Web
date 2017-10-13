namespace HttpServer.Server.Handlers
{
    using Contracts;
    using Enums;
    using HTTP.Contracts;
    using Routing.Contracts;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Server.HTTP.Response;

    public class HttpHandler : IRequestHandler
    {
        private readonly IServerRouteConfig serverRouteConfig;
        
        public HttpHandler(IServerRouteConfig serverRouteConfig)
        {
            this.serverRouteConfig = serverRouteConfig;
        }

        public IHttpResponse Handle(IHttpContext context)
        {
            HttpRequestMethod requestMethod = context.Request.Method;
            string requestPath = context.Request.Path;
            IDictionary<string, IRoutingContext> registeredRoutes = this.serverRouteConfig.Routes[requestMethod];

            foreach (var registeredRoute in registeredRoutes)
            {
                string routePattern = registeredRoute.Key;
                IRoutingContext routingContext = registeredRoute.Value;

                Regex routeRegex = new Regex(routePattern);
                Match routeMatch = routeRegex.Match(requestPath);

                if (!routeMatch.Success)
                {
                    continue;
                }

                var parameters = routingContext.Parameters;

                foreach (var parameter in parameters)
                {
                    var parameterValue = routeMatch.Groups[parameter].Value;

                    context.Request.AddUrlParameter(parameter, parameterValue);
                }

                return routingContext.Handler.Handle(context);
            }

            return new NotFoundResponse();
        }
    }
}
