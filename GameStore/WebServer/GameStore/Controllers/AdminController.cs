namespace WebServer.GameStore.Controllers
{
    using Service;
    using Service.Contracts;
    using Server.HTTP.Contracts;
    using Server.HTTP;
    using Server.HTTP.Response;
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using ViewModels.Game;
    using System.Text;

    public class AdminController : Controller
    {
        private const string ListPath = @"Admin\list";
        private const string AddGamePath = @"Admin\add-game";

        private readonly IUserService userService;
        private readonly IGameService gameService;
        private string returnPath = string.Empty;

        public AdminController(IHttpRequest request) 
            : base(request)
        {
            this.userService = new UserService();
            this.gameService = new GameService();
        }

        public IHttpResponse List()
        {
            if (!this.IsAuthenticated)
            {
                return new RedirectResponse("/login");
            }

            if (!this.IsAdmin)
            {
                return new RedirectResponse("/");
            }

            var allGames = this.gameService.List();

            if (!allGames.Any())
            {
                this.ViewData["showGames"] = Controller.AddHideElementClass;
                this.ViewData["showMessage"] = Controller.RemoveHideElementClass;
                this.ViewData["gamesList"] = Controller.RemoveHideElementClass;
                this.ViewData["message"] = "No games in the Database";
            }
            else
            {
                string result = this.RenderTable(allGames);

                this.ViewData["showGames"] = Controller.RemoveHideElementClass;
                this.ViewData["showMessage"] = Controller.AddHideElementClass;
                this.ViewData["message"] = Controller.RemoveHideElementClass; 
                this.ViewData["gamesList"] = result;
            }

            return this.FileViewResponse(ListPath);
        }
        
        public IHttpResponse Add()
        {
            if (!this.IsAuthenticated)
            {
                return new RedirectResponse("/login");
            }

            if (!this.IsAdmin)
            {
                return new RedirectResponse("/");
            }

            return this.FileViewResponse(AddGamePath);

        }
        
        public IHttpResponse Add(AddGameViewModel model)
        {
            if (!this.ValidateModel(model))
            {
                return this.FileViewResponse(AddGamePath);
            }

            //validation of title, price and size
            //todo add to db
            return new RedirectResponse("/admin/games");
        }

        //get
        public IHttpResponse Delete(int id)
        {
            
            throw new NotImplementedException();
        }

        //get
        public IHttpResponse Edit(int id)
        {
            throw new NotImplementedException();
        }

        private string RenderTable(IList<ListGamesViewModel> allGames)
        {
            var resultString = new StringBuilder();

            for (int i = 0; i < allGames.Count; i++)
            {
                var gameString = new StringBuilder();

                var id = allGames[i].Id;
                var name = allGames[i].Title;
                var price = $"{allGames[i].Price} &euro;";
                var size = $"{allGames[i].Size} Gb";

                if (i % 2 == 0)
                {
                    gameString.AppendLine("<tr class=\"table-warning\">");
                }
                else
                {
                    gameString.AppendLine("<tr>");
                }

                gameString.AppendLine($"<th scope=\"row\">{id}</th>");
                gameString.AppendLine($"    <td>{name}</td>");
                gameString.AppendLine($"    <td>{size}</td>");
                gameString.AppendLine($"    <td>{price}</td>");
                gameString.AppendLine($"    <td>");
                gameString.AppendLine($"        <a href=\"/admin/games/edit?id={id}\" class=\"btn btn-warning btn-sm\">Edit</a>");
                gameString.AppendLine($"        <a href=\"/admin/games/delete?id={id}\" class=\"btn btn-danger btn-sm\">Delete</a>");
                gameString.AppendLine($"    </td>");
                gameString.AppendLine("/<tr>");

                resultString.AppendLine(gameString.ToString());
            }

            return resultString.ToString();
        }  
    }
}
