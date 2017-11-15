namespace WebServer.ByTheCake
{
    using Controllers;
    using Server.Contracts;
    using Server.Handlers;
    using Server.Routing.Contracts;

    public class MainApplication : IApplication
    {
        public void Start(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.
                AddRoute("/", new GetHandler(ctx => new HomeController().Index()));

            appRouteConfig.
                AddRoute("/about", new GetHandler(ctx => new HomeController().About()));

            appRouteConfig.
                AddRoute("/add", new GetHandler(ctx => new CakesController().Add()));

            appRouteConfig.
                AddRoute("/add", new PostHandler(ctx => new CakesController().Add(ctx.Request.FormData)));

            appRouteConfig.
                AddRoute("/search", new GetHandler(ctx => new CakesController().Search(ctx.Request.QueryParameters)));

            appRouteConfig.
                AddRoute("/calculator", new GetHandler(ctx => new CalculatorController().Calculate()));

            appRouteConfig.
                AddRoute("/calculator", new PostHandler(ctx => new CalculatorController().Calculate(ctx.Request.FormData)));

            appRouteConfig.
                AddRoute("/loginn", new GetHandler(ctx => new UserController().Loginn()));

            appRouteConfig.
                AddRoute("/loginn", new PostHandler(ctx => new UserController().Loginn(ctx.Request.FormData)));

            appRouteConfig.
                AddRoute("/login", new GetHandler(ctx => new UserController().Login()));

            //appRouteConfig.
            //    AddRoute("/login", new PostHandler(ctx => new UserController().Login(ctx.Request.FormData)));

            appRouteConfig.
                AddRoute("/login", new PostHandler(ctx => new UserController().Login(ctx.Request)));

            appRouteConfig.
                AddRoute("/email", new GetHandler(ctx => new UserController().Email()));

            appRouteConfig.
                AddRoute("/email", new PostHandler(ctx => new UserController().Email(ctx.Request.FormData)));

            appRouteConfig.
                AddRoute("/greeting", new GetHandler(ctx => new UserController().Greeting(ctx.Request.Session)));

            appRouteConfig.
                AddRoute("/greeting", new PostHandler(ctx => new UserController().Greeting(ctx.Request)));

            appRouteConfig
                .AddRoute(@"/Images/{(?<imagePath>[a-zA-Z0-9_]+\.(jpg|png))}", new GetHandler(ctx => new HomeController().Image(ctx.Request.UrlParameters["imagePath"])));
        }
    }
}
