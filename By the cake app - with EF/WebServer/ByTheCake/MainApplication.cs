namespace WebServer.ByTheCake
{
    using Controllers;
    using Server.Contracts;
    using Server.Handlers;
    using Server.Routing.Contracts;
    using ViewModels.User;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using WebServer.ByTheCake.ViewModels.Products;

    public class MainApplication : IApplication
    {
        public void InitializeDb()
        {
            using (var db = new ByTheCakeDbContext())
            {
                db.Database.Migrate();
            }
        }

        public void Start(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig
                .AddRoute("/", new GetHandler(ctx => new HomeController().Index()));

            appRouteConfig
                .AddRoute("/about", new GetHandler(ctx => new HomeController().About()));

            appRouteConfig
                .AddRoute("/register", new GetHandler(ctx => new UserController().Register()));

            appRouteConfig
                .AddRoute("/register", new PostHandler(ctx => new UserController().Register(ctx.Request, new RegisterUserViewModel()
                    {
                        Username = ctx.Request.FormData["username"],
                        Password = ctx.Request.FormData["password"],
                        ConfirmPassword = ctx.Request.FormData["conf-password"]
                    })));

            appRouteConfig
                .AddRoute("/profile", new GetHandler(ctx => new UserController().Profile(ctx.Request)));

            appRouteConfig
                .AddRoute("/login", new GetHandler(ctx => new UserController().Login()));

            appRouteConfig
                .AddRoute("/login", new PostHandler(ctx => new UserController().Login(ctx.Request, new LoginUserViewModel()
                    {
                        Username = ctx.Request.FormData["username"],
                        Password = ctx.Request.FormData["password"]
                    })));

            appRouteConfig
                 .AddRoute("/logout", new PostHandler(ctx => new UserController().Logout(ctx.Request)));

            appRouteConfig
                .AddRoute("/add", new GetHandler(ctx => new ProductsController().Add()));

            appRouteConfig
                .AddRoute("/add", new PostHandler(ctx => new ProductsController().Add(new AddProductViewModel()
                {
                    Name = ctx.Request.FormData["name"],
                    Price = decimal.Parse(ctx.Request.FormData["price"]),
                    ImageUrl = ctx.Request.FormData["url"]
                })));

            appRouteConfig
                .AddRoute("/products/{(?<id>[0-9]+)}", new GetHandler(ctx => new ProductsController().Details(int.Parse(ctx.Request.UrlParameters["id"]))));

            appRouteConfig
                .AddRoute("/orders", new GetHandler(ctx => new ProductsController().AllOrders(ctx.Request)));

            appRouteConfig
                .AddRoute("/order", new GetHandler(ctx => new ProductsController().Order(ctx.Request)));           

            appRouteConfig
                .AddRoute("/cart", new GetHandler(ctx => new ProductsController().ShowCart(ctx.Request)));

            appRouteConfig
                .AddRoute("/search", new GetHandler(ctx => new ProductsController().Search(ctx.Request)));

            appRouteConfig
                .AddRoute("/success", new PostHandler(ctx => new ProductsController().Success(ctx.Request)));

            appRouteConfig
               .AddRoute(@"/Images/{(?<imagePath>[a-zA-Z0-9_]+\.(jpg|png))}",
                    new GetHandler(ctx => new HomeController().Image(ctx.Request.UrlParameters["imagePath"])));
        }
    }
}
