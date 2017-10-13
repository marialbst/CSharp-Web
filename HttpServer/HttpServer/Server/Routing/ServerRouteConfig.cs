namespace HttpServer.Server.Routing
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Contracts;
    using Enums;
    using Handlers;
   
    public class ServerRouteConfig : IServerRouteConfig
    {
        private readonly IDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> routes;

        public ServerRouteConfig(IAppRouteConfig appRouteConfig)
        {
            this.routes = new Dictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>>();

            var methods = Enum
               .GetValues(typeof(HttpRequestMethod))
               .Cast<HttpRequestMethod>();

            foreach (HttpRequestMethod method in methods)
            {
                this.routes[method] = new Dictionary<string, IRoutingContext>();
            }

            this.InitializeServerConfig(appRouteConfig);
        }

        public IDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> Routes { get { return this.routes; } }

        private void InitializeServerConfig(IAppRouteConfig appRouteConfig)
        {
            foreach (var registeredRoute in appRouteConfig.Routes)
            {
                HttpRequestMethod method = registeredRoute.Key;
                var routesWithHandler = registeredRoute.Value;

                foreach (var routeWithHandler in routesWithHandler)
                {
                    string route = routeWithHandler.Key;

                    RequestHandler handler = routeWithHandler.Value;

                    IList<string> parameters = new List<string>();

                    string parsedRouteRegex = this.ParseRoute(route,parameters);

                    IRoutingContext context = new RoutingContext(handler, parameters);

                    this.routes[method].Add(parsedRouteRegex, context);
                }
            }
        }

        private string ParseRoute(string route, IList<string> parameters)
        {
            StringBuilder result = new StringBuilder();
            result.Append("^");

            if (route == "/")
            {
                result.Append("/$");
                return result.ToString();
            }

            string[] tokens = route.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            this.ParseTokens(parameters, tokens, result);

            return result.ToString();
        }

        private void ParseTokens(IList<string> parameters, string[] tokens, StringBuilder result)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                string endChar = i == tokens.Length - 1 ? "$" : "/";

                string currentToken = tokens[i];

                if (!currentToken.StartsWith("{") && !currentToken.EndsWith("}"))
                {
                    result.Append($"{currentToken}{endChar}");
                    continue;
                }

                Regex parameterRegex = new Regex("<\\w+>");
                Match parameterMatch = parameterRegex.Match(currentToken);

                if (!parameterMatch.Success)
                {
                    throw new InvalidOperationException($"Route parameter in '{currentToken}' is invalid.");
                }

                string match = parameterMatch.Value;
                string parameterName = match.Substring(1, match.Length - 2);

                parameters.Add(parameterName);

                string currentTokenRemovedBrackets = currentToken.Substring(1, currentToken.Length - 2);
                result.Append($"{currentTokenRemovedBrackets}{endChar}");
            }
        }
    }
}
