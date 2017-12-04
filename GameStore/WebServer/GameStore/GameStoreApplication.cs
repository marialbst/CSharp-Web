namespace WebServer.GameStore
{
    using System;
    using System.Globalization;
    using Controllers;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Server.Contracts;
    using Server.Handlers;
    using Server.Routing.Contracts;
    using ViewModels.Account;
    using WebServer.GameStore.ViewModels.Game;

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
            //appRouteConfig.AnonymousPaths.Add("/");
            //appRouteConfig.AnonymousPaths.Add("/register");
            //appRouteConfig.AnonymousPaths.Add("/login");
            //appRouteConfig.AnonymousPaths.Add("/games/details/{(?<id>[0-9]+)}");

            appRouteConfig
                .AddRoute("/", new GetHandler(ctx => new HomeController(ctx.Request).Index()));

            appRouteConfig
               .AddRoute("/", new PostHandler(ctx => new HomeController(ctx.Request).Index(ctx.Request.FormData["filter"])));

            appRouteConfig
                .AddRoute("/register", new GetHandler( ctx => new AccountController(ctx.Request).Register()));

            appRouteConfig
               .AddRoute("/register", new PostHandler(ctx => new AccountController(ctx.Request)
                    .Register(new RegisterViewModel()
                    {
                        Email = ctx.Request.FormData["email"].Trim(),
                        FullName = ctx.Request.FormData["fullName"].Trim(),
                        Password = ctx.Request.FormData["password"].Trim(),
                        ConfirmPassword = ctx.Request.FormData["confirm-password"].Trim()
                    })));

            appRouteConfig
                .AddRoute("/login", new GetHandler(ctx => new AccountController(ctx.Request).Login()));

            appRouteConfig
               .AddRoute("/login", new PostHandler(ctx => new AccountController(ctx.Request)
               .Login(new LoginViewModel()
               {
                   Email = ctx.Request.FormData["email"].Trim(),
                   Password = ctx.Request.FormData["password"].Trim()
               })));

            appRouteConfig
                 .AddRoute("/logout", new GetHandler(ctx => new AccountController(ctx.Request).Logout()));

            appRouteConfig
               .AddRoute("/admin/games", new GetHandler(ctx => new AdminController(ctx.Request).List()));

            appRouteConfig
                .AddRoute("/admin/games/add", new GetHandler(ctx => new AdminController(ctx.Request).Add()));

            appRouteConfig
                .AddRoute("/admin/games/add", new PostHandler(ctx => new AdminController(ctx.Request).Add(new AddGameViewModel()
                {
                    Title = ctx.Request.FormData["name"],
                    Description = ctx.Request.FormData["description"],
                    ImageThumbnail = ctx.Request.FormData["url"],
                    Price = Math.Round(decimal.Parse(ctx.Request.FormData["price"]), 2),
                    Size = Math.Round(double.Parse(ctx.Request.FormData["size"]), 1),
                    TrailerId = ctx.Request.FormData["youtube-id"],
                    ReleaseDate = DateTime.ParseExact(ctx.Request.FormData["release-date"], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                })));

            appRouteConfig
                .AddRoute("/admin/games/edit/{(?<id>[0-9]+)}", new GetHandler(ctx => new AdminController(ctx.Request).Edit(int.Parse(ctx.Request.UrlParameters["id"]))));

            appRouteConfig
                .AddRoute("/admin/games/edit/{(?<id>[0-9]+)}", new PostHandler(ctx => new AdminController(ctx.Request).Edit(new ManageGameViewModel()
                {
                    Id = int.Parse(ctx.Request.UrlParameters["id"]),
                    Title = ctx.Request.FormData["name"],
                    Description = ctx.Request.FormData["description"],
                    ImageThumbnail = ctx.Request.FormData["url"],
                    Price = Math.Round(decimal.Parse(ctx.Request.FormData["price"]), 2),
                    Size = Math.Round(double.Parse(ctx.Request.FormData["size"]), 1),
                    TrailerId = ctx.Request.FormData["youtube-id"],
                    ReleaseDate = DateTime.ParseExact(ctx.Request.FormData["release-date"], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                })));

            appRouteConfig
                .AddRoute("/admin/games/delete/{(?<id>[0-9]+)}", new GetHandler(ctx => new AdminController(ctx.Request).Delete(int.Parse(ctx.Request.UrlParameters["id"]))));

            appRouteConfig
                .AddRoute("/admin/games/delete/{(?<id>[0-9]+)}", new PostHandler(ctx => new AdminController(ctx.Request).DeleteConfirmed(int.Parse(ctx.Request.UrlParameters["id"]))));

            appRouteConfig
                .AddRoute("/games/details/{(?<id>[0-9]+)}", new GetHandler(ctx => new GameController(ctx.Request).Details(int.Parse(ctx.Request.UrlParameters["id"]))));

            appRouteConfig
                .AddRoute("/cart", new GetHandler(ctx => new GameController(ctx.Request).Cart()));

            appRouteConfig
                .AddRoute("/cart", new PostHandler(ctx => new GameController(ctx.Request).Order()));

           appRouteConfig
                .AddRoute("/cart/add/{(?<id>[0-9]+)}", new GetHandler(ctx => new GameController(ctx.Request).CartAdd(int.Parse(ctx.Request.UrlParameters["id"]))));

            appRouteConfig
                .AddRoute("/cart/remove/{(?<id>[0-9]+)}", new GetHandler(ctx => new GameController(ctx.Request).CartRemove(int.Parse(ctx.Request.UrlParameters["id"]))));

            appRouteConfig
               .AddRoute(@"/Images/{(?<imagePath>[a-zA-Z0-9_]+\.(jpg|png))}",
                    new GetHandler(ctx => new HomeController(ctx.Request).Image(ctx.Request.UrlParameters["imagePath"])));
        }
    }
}
