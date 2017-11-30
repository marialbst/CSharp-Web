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
        private const string EditGamePath = @"Admin\edit-game";
        private const string DeleteGamePath = @"Admin\delete-game";

        private readonly IUserService userService;
        private readonly IGameService gameService;

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

            this.gameService.Create(
                model.Title, 
                model.TrailerId, 
                model.ImageThumbnail, 
                model.Size,
                model.Price, 
                model.Description, 
                model.ReleaseDate.Value);

            return new RedirectResponse("/admin/games");
        }

        //get
        public IHttpResponse Edit(int id)
        {
            if (!this.IsAdmin)
            {
                return new RedirectResponse("/");
            }

            var model = this.gameService.Find(id);

            if (model == null)
            {
                this.ViewData["showMessage"] = Controller.RemoveHideElementClass;
                this.ViewData["message"] = "Game not found";

                return new RedirectResponse("/admin/games");
            }

            this.PopulateFormWithDbData(model);

            return this.FileViewResponse(EditGamePath);
        }

        public IHttpResponse Edit(ManageGameViewModel model)
        {
            if (!this.ValidateModel(model))
            {
                return this.Edit(model.Id);
            }

            bool result = this.gameService.Edit(
                model.Id,
                model.Title,
                model.TrailerId,
                model.ImageThumbnail,
                model.Size,
                model.Price,
                model.Description,
                model.ReleaseDate.Value);            

            if (!result)
            {
                this.ViewData["showError"] = Controller.RemoveHideElementClass;
                this.ViewData["error"] = "No changes has been made";
                return this.Edit(model.Id);
            }

            return new RedirectResponse("/admin/games");
        }

        //get
        public IHttpResponse Delete(int id)
        {
            if (!this.IsAdmin)
            {
                return new RedirectResponse("/");
            }

            var model = this.gameService.Find(id);

            if (model == null)
            {
                this.AddError("Game not found");

                return new RedirectResponse("/admin/games");
            }

            this.PopulateFormWithDbData(model);

            return this.FileViewResponse(DeleteGamePath);
        }

        public IHttpResponse DeleteConfirmed(int id)
        {
            bool isExisting = this.gameService.Exists(id);

            if (!isExisting)
            {
                this.AddError("Game not found");

                return new RedirectResponse("/admin/games");
            }

            this.gameService.Delete(id);

            return new RedirectResponse("/admin/games");
        }

        private void PopulateFormWithDbData(AddGameViewModel model)
        {
            this.ViewData["nameValue"] = model.Title;
            this.ViewData["descriptionValue"] = model.Description;
            this.ViewData["urlValue"] = model.ImageThumbnail;
            this.ViewData["priceValue"] = model.Price.ToString();
            this.ViewData["sizeValue"] = model.Size.ToString();
            this.ViewData["youtubeValue"] = model.TrailerId;
            this.ViewData["dateValue"] = String.Format("{0:yyyy-MM-dd}", model.ReleaseDate);
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
                gameString.AppendLine($"        <a href=\"/admin/games/edit/{id}\" class=\"btn btn-warning btn-sm\">Edit</a>");
                gameString.AppendLine($"        <a href=\"/admin/games/delete/{id}\" class=\"btn btn-danger btn-sm\">Delete</a>");
                gameString.AppendLine($"    </td>");
                gameString.AppendLine("</tr>");

                resultString.AppendLine(gameString.ToString());
            }

            return resultString.ToString();
        }  
    }
}
