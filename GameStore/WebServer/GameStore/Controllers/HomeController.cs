namespace WebServer.GameStore.Controllers
{
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;
    using Service.Contracts;
    using Service;
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using WebServer.GameStore.ViewModels.Game;
    using System.Text;
    using WebServer.Server.HTTP;

    public class HomeController : Controller
    {
        private const string IndexPath = @"Home\index";

        private readonly IGameService gameService;
        private readonly IUserService userService;

        public HomeController(IHttpRequest request)
            :base(request)
        {
            this.gameService = new GameService();
            this.userService = new UserService();
        }

        public IHttpResponse Index()
        {
            var gamesList = this.gameService.All();

            if (!gamesList.Any())
            {
                this.ViewData["games"] = "<div class=\"text-center\"><h1 class=\"display-3\">No games in the database</h1></div>";
                return this.FileViewResponse(IndexPath);
            }

            this.ViewData["games"] = this.RenderGamesHtml(gamesList);

            return this.FileViewResponse(IndexPath);
        }

        public IHttpResponse Index(string filter)
        {
            if(filter == "All")
            {
                return this.Index();
            }

            var currentUser = this.Request.Session.Get<string>(SessionStore.CurrentUserKey);

            if(currentUser == null)
            {
                return new RedirectResponse("/login");
            }

            var user = this.userService.Find(currentUser);

            if(user == null)
            {
                return this.Index();
            }

            var gamesList = this.gameService.All(user.Id);

            if (!gamesList.Any())
            {
                this.ViewData["games"] = "<div class=\"text-center\"><h2 class=\"display-6\">You don't have any games</h2></div>";
                return this.FileViewResponse(IndexPath);
            }

            this.ViewData["games"] = this.RenderGamesHtml(gamesList);

            return this.FileViewResponse(IndexPath);
        }

        public IHttpResponse Image(string imagePath)
        {
            return new ImageResponse(imagePath);
        }

        private string RenderGamesHtml(IList<IndexViewGame> gamesList)
        {
            var result = new StringBuilder();

            for (int i = 0; i < gamesList.Count; i++)
            {
                if (i % 3 == 0)
                {
                    result.AppendLine("<div class=\"card-group\">");
                }

                result.AppendLine("<div class=\"card col-md-4 thumbnail\">");

                result.AppendLine($"<img class=\"card-image-top img-fluid img-thumbnail\" src=\"{gamesList[i].ImageThumbnail}\"/>");

                result.AppendLine("<div class=\"card-body\">");
                result.AppendLine($"<h4 class=\"card-title\">{gamesList[i].Title}</h4>");
                result.AppendLine($"<p class=\"card-text\"><strong>Price</strong> - {gamesList[i].Price}&euro;</p>");
                result.AppendLine($"<p class=\"card-text\"><strong>Size</strong> - {gamesList[i].Size}Gb</p>");
                result.AppendLine($"<p class=\"card-text\">{gamesList[i].Description.Substring(0,300)}...</p>");
                result.AppendLine("</div>");

                result.AppendLine("<div class=\"card-footer\">");

                if (this.IsAuthenticated && this.IsAdmin)
                {
                    result.AppendLine($"<a class=\"card-button btn btn-warning\" name=\"edit\" href=\"/admin/games/edit/{gamesList[i].Id}\">Edit</a>");
                    result.AppendLine($"<a class=\"card-button btn btn-danger\" name=\"delete\" href=\"/admin/games/delete/{gamesList[i].Id}\">Delete</a>");
                }

                result.AppendLine($"<a class=\"card-button btn btn-outline-primary\" name=\"info\" href=\"/games/details/{gamesList[i].Id}\">Info</a>");
                result.AppendLine($"<a class=\"card-button btn btn-primary\" name=\"buy\" href=\"/cart/add/{gamesList[i].Id}\">Buy</a>");
                result.AppendLine("</div></div>");

                if (i % 3 == 2)
                {
                    result.AppendLine("</div>");
                }
            }

            return result.ToString();
        }
    }
}
