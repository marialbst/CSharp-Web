namespace WebServer.Application
{
    using Controllers;
    using Server.Contracts;
    using Server.Handlers;
    using Server.Routing.Contracts;

    public class MainApplication : IApplication
    {
        public void Start(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig
                .AddRoute("/", new GetHandler(httpContext => new HomeController().Index()));

            appRouteConfig
                .AddRoute("/register", new PostHandler(httpContext => new UserController().RegisterPost(httpContext.Request.FormData["name"])));

            appRouteConfig
                .AddRoute("/register", new GetHandler(httpContext => new UserController().RegisterGet()));
            
            appRouteConfig
                .AddRoute("/user/{(?<name>[a-z]+)}", new GetHandler(httpContext => new UserController().Details(httpContext.Request.UrlParameters["name"])));

            appRouteConfig.
                AddRoute("/testsession", new GetHandler(httpContext => new HomeController().SessionTest(httpContext.Request)));
        }
    }
}
