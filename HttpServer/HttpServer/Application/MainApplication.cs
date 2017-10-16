namespace HttpServer.Application
{
    using Server.Routing.Contracts;
    using Server.Contracts;
    using Server.Handlers;
    using Controllers;

    public class MainApplication : IApplication
    {
        public void Start(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig
                .Get("/", handler => new HomeController().Index());

            appRouteConfig
                .Get("/register", handler => new UserController().RegisterGet());

            appRouteConfig
                .Post("/register", handler => new UserController().RegisterPost(handler.Request.FormData["name"]));

            appRouteConfig
                .AddRoute("/user/{?<name>[a-z]+}", new GetHandler(ctx => new UserController().UserDetails(ctx.Request.UrlParameters["name"])));
        }
    }
}
