namespace WebServer.GameStore
{
    using Controllers;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Server.Contracts;
    using Server.Handlers;
    using Server.Routing.Contracts;
    using ViewModels.Account;

    public class GameStoreApplication : IApplication
    {
        public void InitializeDb()
        {
            using (var db = new GameStoreDbContext())
            {
                db.Database.Migrate();
            }
        }

        public void Start(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.AnonymousPaths.Add("/");
            appRouteConfig.AnonymousPaths.Add("/register");
            appRouteConfig.AnonymousPaths.Add("/login");

            appRouteConfig
                .AddRoute("/", new GetHandler(ctx => new HomeController().Index(ctx.Request.Session)));

            appRouteConfig
                .AddRoute("/register", new GetHandler( ctx => new AccountController().Register(ctx.Request.Session)));

            appRouteConfig
               .AddRoute("/register", new PostHandler(ctx => new AccountController()
                    .Register(ctx.Request, new RegisterViewModel()
                    {
                        Email = ctx.Request.FormData["email"].Trim(),
                        FullName = ctx.Request.FormData["fullName"].Trim(),
                        Password = ctx.Request.FormData["password"].Trim(),
                        ConfirmPassword = ctx.Request.FormData["confirm-password"].Trim()
                    })));

            appRouteConfig
                .AddRoute("/login", new GetHandler(ctx => new AccountController().Login(ctx.Request.Session)));

            appRouteConfig
               .AddRoute("/login", new PostHandler(ctx => new AccountController().Login(ctx.Request, new LoginViewModel()
               {
                   Email = ctx.Request.FormData["email"].Trim(),
                   Password = ctx.Request.FormData["password"].Trim()
               })));

            appRouteConfig
                 .AddRoute("/logout", new GetHandler(ctx => new AccountController().Logout(ctx.Request)));

            appRouteConfig
               .AddRoute(@"/Images/{(?<imagePath>[a-zA-Z0-9_]+\.(jpg|png))}",
                    new GetHandler(ctx => new HomeController().Image(ctx.Request.UrlParameters["imagePath"])));
        }
    }
}
