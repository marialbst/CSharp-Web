namespace WebServer.Server.Handlers
{
    using System.Text.RegularExpressions;
    using Contracts;
    using HTTP.Contracts;
    using Routing.Contracts;
    using Enums;
    using HTTP.Response;
    using Application.Views;
    using Server.HTTP;

    public class HttpHandler : IRequestHandler
    {
        private readonly IServerRouteConfig serverRouteConfig;

        public HttpHandler(IServerRouteConfig serverRouteConfig)
        {
            this.serverRouteConfig = serverRouteConfig;
        }

        public IHttpResponse Handle(IHttpContext httpContext)
        {
            string loginPath = "/login";

            if (httpContext.Request.Path != loginPath &&
                !httpContext.Request.Session.IsAuthenticated())
            {
                return new RedirectResponse(loginPath);
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
