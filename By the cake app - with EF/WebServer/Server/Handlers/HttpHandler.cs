namespace WebServer.Server.Handlers
{
    using System.Text.RegularExpressions;
    using Contracts;
    using HTTP.Contracts;
    using Routing.Contracts;
    using Enums;
    using HTTP.Response;
    using Server.HTTP;
    using System.Linq;

    public class HttpHandler : IRequestHandler
    {
        private readonly IServerRouteConfig serverRouteConfig;

        public HttpHandler(IServerRouteConfig serverRouteConfig)
        {
            this.serverRouteConfig = serverRouteConfig;
        }

        public IHttpResponse Handle(IHttpContext httpContext)
        {
            var anonymousPaths = new[] { "/login", "/register" };

            if (!anonymousPaths.Contains(httpContext.Request.Path) && (httpContext.Request.Session == null ||
                !httpContext.Request.Session.IsAuthenticated()))
            {
                return new RedirectResponse(anonymousPaths.First());
            }

            var routesContext = this.serverRouteConfig.Routes[httpContext.Request.Method];
            string path = httpContext.Request.Path;

            foreach (var route in routesContext)
            {
                string pattern = route.Key;
                IRoutingContext routingContext = route.Value;

                Regex regex = new Regex(pattern);

                Match match = regex.Match(path);

                if (!match.Success)
                {
                    continue;
                }

                foreach (var parameter in routingContext.Parameters)
                {
                    httpContext.Request.AddUrlParameter(parameter, match.Groups[parameter].Value);
                }

                return routingContext.RequestHandler.Handle(httpContext);
            }

            return new ViewResponse(HttpStatusCode.NotFound, new NotFoundView());
        }
    }
}
