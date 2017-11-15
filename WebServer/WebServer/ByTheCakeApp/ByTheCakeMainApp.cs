namespace WebServer.ByTheCakeApp
{
    using Controllers;
    using Server.Contracts;
    using Server.Routing.Contracts;
    using Server.Handlers;

    public class ByTheCakeMainApp : IApplication
    {
        public void Start(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig
                .AddRoute("/", new GetHandler(context => new HomeController().Index()));

            appRouteConfig
                .AddRoute("/about", new GetHandler(context => new HomeController().About()));

            appRouteConfig
                .AddRoute("/add", new GetHandler(context => new CakeController().AddGet()));

            appRouteConfig
               .AddRoute("/add", new PostHandler(context => new CakeController().AddPost(context.Request.FormData["name"], context.Request.FormData["price"])));

            appRouteConfig
               .AddRoute("/search", new GetHandler(context => new CakeController().Search(context.Request.QueryParameters)));

            appRouteConfig
                .AddRoute("/calculator", new GetHandler(context => new CalculatorController().CalculateGet()));

            appRouteConfig
                .AddRoute("/calculator", new PostHandler(context => new CalculatorController().CalculatePost(context.Request.FormData["number1"], context.Request.FormData["operator"], context.Request.FormData["number2"])));

            appRouteConfig
                .AddRoute("/login1", new GetHandler(context => new UserController().LoginGet()));

            appRouteConfig
                .AddRoute("/login1", new PostHandler(context => new UserController().LoginPost(context.Request.FormData["username"], context.Request.FormData["password"])));

            appRouteConfig
                .AddRoute("/login", new GetHandler(context => new HomeController().Login()));

            appRouteConfig
                .AddRoute("/login", new PostHandler(context => new HomeController().Login(context.Request)));

            appRouteConfig
                .AddRoute("/email", new GetHandler(context => new UserController().EmailGet()));

            appRouteConfig
                .AddRoute("/email", new PostHandler(context => new UserController().EmailPost(context.Request.FormData)));

            appRouteConfig
                .AddRoute("/success", new GetHandler(context => new UserController().Success()));
        }
    }
}
