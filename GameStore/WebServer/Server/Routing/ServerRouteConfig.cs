namespace WebServer.Server.Routing
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Contracts;
    using Enums;
    using Handlers;
    using System.Text;
    using System.Text.RegularExpressions;

    public class ServerRouteConfig : IServerRouteConfig
    {
        public ServerRouteConfig(IAppRouteConfig routeConfig)
        {
            this.Routes = new Dictionary<HttpRequestMethod, Dictionary<string, IRoutingContext>>();
            this.AnonymousPaths = new List<string>(routeConfig.AnonymousPaths);

            var methods = Enum.GetValues(typeof(HttpRequestMethod)).Cast<HttpRequestMethod>();

            foreach (var method in methods)
            {
                this.Routes.Add(method, new Dictionary<string, IRoutingContext>());
            }

            this.InitializeServerConfig(routeConfig);
        }

        public Dictionary<HttpRequestMethod, Dictionary<string, IRoutingContext>> Routes { get; }

        public ICollection<string> AnonymousPaths { get; private set; }

        private void InitializeServerConfig(IAppRouteConfig routeConfig)
        {
            foreach (var regRoute in routeConfig.Routes)
            {
                HttpRequestMethod method = regRoute.Key;
                Dictionary<string, RequestHandler> routesWithHandler = regRoute.Value;

                foreach (var kvp in routesWithHandler)
                {
                    string route = kvp.Key;
                    RequestHandler handler = kvp.Value;

                    IList<string> parameters = new List<string>();
                    string parsedRegex = this.ParseRoute(route, parameters);

                    IRoutingContext routingContext = new RoutingContext(handler, parameters);

                    this.Routes[method].Add(parsedRegex, routingContext);
                }
            }
        }

        private string ParseRoute(string route, IList<string> parameters)
        {
            StringBuilder parsedRegex = new StringBuilder();

            parsedRegex.Append("^/");

            if (route == "/")
            {
                parsedRegex.Append($"$");
                return parsedRegex.ToString();
            }

            string[] tokens = route.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            this.ParseTokens(parameters,tokens,parsedRegex);

            return parsedRegex.ToString();
        }

        private void ParseTokens(IList<string> parameters, string[] tokens, StringBuilder parsedRegex)
        {
            for (int idx = 0; idx < tokens.Length; idx++)
            {
                string end = idx == tokens.Length - 1 ? "$" : "/";
                if (!tokens[idx].StartsWith("{") && !tokens[idx].EndsWith("}"))
                {
                    parsedRegex.Append($"{tokens[idx]}{end}");
                    continue;
                }

                string pattern = "<\\w+>";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(tokens[idx]);

                if (!match.Success)
                {
                    continue;
                }

                string paramName = match.Groups[0].Value.Substring(1, match.Groups[0].Length - 2);
                parameters.Add(paramName);
                parsedRegex.Append($"{tokens[idx].Substring(1, tokens[idx].Length - 2)}{end}");
            }
        }
    }
}
